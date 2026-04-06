import type { BackgroundImage } from '../models.js';
import { BackgroundImageFillMode, HorizontalAlignment, VerticalAlignment } from '../enums.js';

/** Fluent builder for {@link BackgroundImage}. */
export class BackgroundImageBuilder {
  private readonly bg: BackgroundImage = {};

  /** Sets the URL of the background image. @param url The image URL. @returns The builder instance for method chaining. */
  withUrl(url: string): this { this.bg.url = url; return this; }
  /** Sets how the image fills the available space. @param fillMode The fill mode. @returns The builder instance for method chaining. */
  withFillMode(fillMode: BackgroundImageFillMode): this { this.bg.fillMode = fillMode; return this; }
  /** Sets the horizontal alignment of the background image. @param alignment The horizontal alignment. @returns The builder instance for method chaining. */
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.bg.horizontalAlignment = alignment; return this; }
  /** Sets the vertical alignment of the background image. @param alignment The vertical alignment. @returns The builder instance for method chaining. */
  withVerticalAlignment(alignment: VerticalAlignment): this { this.bg.verticalAlignment = alignment; return this; }

  /** Builds and returns the configured BackgroundImage. @returns The configured BackgroundImage instance. */
  build(): BackgroundImage { return this.bg; }
}
