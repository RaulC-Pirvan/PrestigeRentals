const express = require('express');
const multer = require('multer');
const path = require('path');
const fs = require('fs');

const swaggerUi = require('swagger-ui-express');
const swaggerJSDoc = require('swagger-jsdoc');

const app = express();
const PORT = 3000;

const uploadDir = path.join(__dirname, 'uploads');
if (!fs.existsSync(uploadDir)) {
  fs.mkdirSync(uploadDir);
}

const storage = multer.diskStorage({
    destination: (req, file, cb) => {
      const { entityType, entityId } = req.body;
      if (!entityType || !entityId) {
        return cb(new Error('Missing entityType or entityId'), null);
      }
  
      const dir = path.join(__dirname, 'uploads', entityType, entityId);
  
      // Make sure directory exists
      fs.mkdirSync(dir, { recursive: true });
  
      cb(null, dir);
    },
    filename: (req, file, cb) => {
      const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
      cb(null, uniqueSuffix + '-' + file.originalname);
    }
  });
const upload = multer({ storage });

// Swagger definition
const swaggerDefinition = {
  openapi: '3.0.0',
  info: {
    title: 'Prestige Rentals Image API',
    version: '1.0.0',
    description: 'API to upload and retrieve images',
  },
  servers: [
    {
      url: `http://localhost:${PORT}`,
    },
  ],
};

// Options for swagger-jsdoc
const options = {
  swaggerDefinition,
  // Path to the API docs
  apis: ['./server.js'],  // <-- We'll add doc comments in this file below
};

// Initialize swagger-jsdoc -> returns validated swagger spec in json format
const swaggerSpec = swaggerJSDoc(options);

// Serve swagger docs at /api-docs
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec));

/**
 * @swagger
 * /upload:
 *   post:
 *     summary: Uploads an image file
 *     requestBody:
 *       required: true
 *       content:
 *         multipart/form-data:
 *           schema:
 *             type: object
 *             properties:
 *               image:
 *                 type: string
 *                 format: binary
 *     responses:
 *       200:
 *         description: Image uploaded successfully
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 message:
 *                   type: string
 *                 filename:
 *                   type: string
 *       400:
 *         description: No file uploaded
 */
app.post('/upload', upload.single('image'), (req, res) => {
    const { entityType, entityId } = req.body;
  
    if (!req.file) return res.status(400).json({ error: 'No file uploaded' });
    if (!entityType || !entityId) return res.status(400).json({ error: 'Missing entityType or entityId' });
  
    res.json({
      message: 'Uploaded',
      filename: req.file.filename,
      path: `/uploads/${entityType}/${entityId}/${req.file.filename}`
    });
  });

app.use('/images', express.static(uploadDir));

app.get('/', (req, res) => {
  res.send(`
    <h1>Upload Image</h1>
    <form method="POST" action="/upload" enctype="multipart/form-data">
      <input type="file" name="image" />
      <button type="submit">Upload</button>
    </form>
    <p>View <a href="/api-docs">Swagger UI</a> for API docs and testing.</p>
  `);
});

app.listen(PORT, () => {
  console.log(`🚀 Server running at http://localhost:${PORT}`);
});
