import type { BackgroundImage } from '../models.js';
import { BackgroundImageFillMode, HorizontalAlignment, VerticalAlignment } from '../enums.js';

/** Fluent builder for {@link BackgroundImage}. */
export class BackgroundImageBuilder {
  private readonly bg: BackgroundImage = {};

  withUrl(url: string): this { this.bg.url = url; return this; }
  withFillMode(fillMode: BackgroundImageFillMode): this { this.bg.fillMode = fillMode; return this; }
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.bg.horizontalAlignment = alignment; return this; }
  withVerticalAlignment(alignment: VerticalAlignment): this { this.bg.verticalAlignment = alignment; return this; }

  build(): BackgroundImage { return this.bg; }
}
