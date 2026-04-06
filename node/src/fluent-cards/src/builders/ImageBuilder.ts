import type { Image, AdaptiveAction } from '../models.js';
import { HorizontalAlignment, ImageSize, ImageStyle, Spacing } from '../enums.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link Image} elements. */
export class ImageBuilder {
  private readonly image: Image = { type: 'Image' };

  withId(id: string): this { this.image.id = id; return this; }
  withUrl(url: string): this { this.image.url = url; return this; }
  withAltText(altText: string): this { this.image.altText = altText; return this; }
  withSize(size: ImageSize): this { this.image.size = size; return this; }
  withStyle(style: ImageStyle): this { this.image.style = style; return this; }
  withWidth(width: string): this { this.image.width = width; return this; }
  withHeight(height: string): this { this.image.height = height; return this; }
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.image.horizontalAlignment = alignment; return this; }
  withBackgroundColor(color: string): this { this.image.backgroundColor = color; return this; }
  withSpacing(spacing: Spacing): this { this.image.spacing = spacing; return this; }
  withSeparator(separator: boolean): this { this.image.separator = separator; return this; }

  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.image.selectAction = b.build();
    return this;
  }

  build(): Image { return this.image; }
}
