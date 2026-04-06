import type { Image, AdaptiveAction } from '../models.js';
import { HorizontalAlignment, ImageSize, ImageStyle, Spacing } from '../enums.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link Image} elements. */
export class ImageBuilder {
  private readonly image: Image = { type: 'Image' };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.image.id = id; return this; }
  /** Sets the URL of the image. @param url The image URL. @returns The builder instance for method chaining. */
  withUrl(url: string): this { this.image.url = url; return this; }
  /** Sets the alternate text for accessibility. @param altText The alt text. @returns The builder instance for method chaining. */
  withAltText(altText: string): this { this.image.altText = altText; return this; }
  /** Sets the size of the image. @param size The image size. @returns The builder instance for method chaining. */
  withSize(size: ImageSize): this { this.image.size = size; return this; }
  /** Sets the display style of the image. @param style The image style. @returns The builder instance for method chaining. */
  withStyle(style: ImageStyle): this { this.image.style = style; return this; }
  /** Sets the explicit width of the image. @param width The width (e.g. `'80px'`). @returns The builder instance for method chaining. */
  withWidth(width: string): this { this.image.width = width; return this; }
  /** Sets the explicit height of the image. @param height The height (e.g. `'80px'`). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.image.height = height; return this; }
  /** Sets the horizontal alignment of the image. @param alignment The horizontal alignment. @returns The builder instance for method chaining. */
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.image.horizontalAlignment = alignment; return this; }
  /** Sets the background color for transparent images. @param color The background color (e.g. `'#FF0000'`). @returns The builder instance for method chaining. */
  withBackgroundColor(color: string): this { this.image.backgroundColor = color; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.image.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator: boolean): this { this.image.separator = separator; return this; }

  /** Sets the action to invoke when the image is selected. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.image.selectAction = b.build();
    return this;
  }

  /** Builds and returns the configured Image. @returns The configured Image instance. */
  build(): Image { return this.image; }
}
