import { readFile, readdir, stat } from 'node:fs/promises';
import { resolve } from 'node:path';

const outputDirectory = resolve('Builds/WebGL');
const html = await readFile(resolve(outputDirectory, 'index.html'), 'utf8');
const buildDirectory = resolve(outputDirectory, 'Build');
const files = await readdir(buildDirectory);

for (const suffix of ['.loader.js', '.data.unityweb', '.framework.js.unityweb', '.wasm.unityweb']) {
  const filename = files.find(file => file.endsWith(suffix));
  if (!filename || !(await stat(resolve(buildDirectory, filename))).size) {
    throw new Error(`Missing WebGL asset: ${suffix}. Run Unity Build > Build Web Game first.`);
  }
  if (!html.includes(filename)) throw new Error(`index.html does not reference ${filename}.`);
}

if (html.includes('{{{')) throw new Error('Unity template placeholders remain in index.html.');
console.log('WebGL build verified. Vercel will serve Builds/WebGL as static files.');
