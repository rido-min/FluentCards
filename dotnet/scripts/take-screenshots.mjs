import { chromium } from 'playwright';
import { spawn } from 'child_process';
import { mkdir } from 'fs/promises';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const repoRoot = path.resolve(__dirname, '..');
const screenshotsDir = path.join(repoRoot, 'docs', 'screenshots');
const projectDir = path.join(repoRoot, 'samples', 'TeamsCardRenderer');
const PORT = 5250;
const BASE_URL = `http://localhost:${PORT}`;

const CARDS = [
  { slug: 'approval',         filename: 'approval.png' },
  { slug: 'status-update',    filename: 'status-update.png' },
  { slug: 'task-update',      filename: 'task-update.png' },
  { slug: 'meeting-reminder', filename: 'meeting-reminder.png' },
  { slug: 'expense-report',   filename: 'expense-report.png' },
];

async function waitForServer(url, retries = 60, delayMs = 1000) {
  for (let i = 0; i < retries; i++) {
    try {
      const res = await fetch(url);
      if (res.ok) return;
    } catch {
      // Not ready yet
    }
    await new Promise(r => setTimeout(r, delayMs));
  }
  throw new Error(`Server at ${url} did not start within ${retries * delayMs}ms`);
}

async function main() {
  await mkdir(screenshotsDir, { recursive: true });

  console.log('Starting TeamsCardRenderer...');
  const server = spawn('dotnet', ['run', '--no-launch-profile'], {
    cwd: projectDir,
    stdio: ['ignore', 'pipe', 'pipe'],
    shell: false,
  });

  server.stdout.on('data', d => process.stdout.write(`[server] ${d}`));
  server.stderr.on('data', d => process.stderr.write(`[server] ${d}`));

  let browser;
  try {
    await waitForServer(`${BASE_URL}/api/cards/approval`);
    console.log('Server ready.');

    browser = await chromium.launch({ headless: true });
    const context = await browser.newContext({
      viewport: { width: 800, height: 900 },
    });

    for (const { slug, filename } of CARDS) {
      console.log(`Rendering ${slug}...`);
      const page = await context.newPage();
      await page.goto(`${BASE_URL}/?card=${slug}`);
      await page.waitForSelector('[data-rendered="true"]', { timeout: 15000 });

      const outputPath = path.join(screenshotsDir, filename);
      await page.locator('#card-container').screenshot({ path: outputPath });
      await page.close();
      console.log(`  Saved: docs/screenshots/${filename}`);
    }

    await context.close();
  } finally {
    browser?.close();
    server.kill('SIGTERM');
    console.log('Done.');
  }
}

main().catch(err => {
  console.error(err);
  process.exit(1);
});
