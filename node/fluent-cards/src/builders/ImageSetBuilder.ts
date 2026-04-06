import type { ImageSet, Image } from '../models.js';
import { ImageSize, Spacing } from '../enums.js';
import { ImageBuilder } from './ImageBuilder.js';

/** Fluent builder for {@link ImageSet} elements. */
export class ImageSetBuilder {
  private readonly imageSet: ImageSet = { type: 'ImageSet', images: [] };

  withId(id: string): this { this.imageSet.id = id; return this; }
  withImageSize(size: ImageSize): this { this.imageSet.imageSize = size; return this; }
  withSpacing(spacing: Spacing): this { this.imageSet.spacing = spacing; return this; }

  addImage(configure: (b: ImageBuilder) => void): this;
  addImage(image: Image): this;
  addImage(configureOrImage: ((b: ImageBuilder) => void) | Image): this {
    if (typeof configureOrImage === 'function') {
      const b = new ImageBuilder();
      configureOrImage(b);
      this.imageSet.images!.push(b.build());
    } else {
      this.imageSet.images!.push(configureOrImage);
    }
    return this;
  }

  build(): ImageSet { return this.imageSet; }
}
