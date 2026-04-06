import type { ImageSet, Image } from '../models.js';
import { ImageSize, Spacing } from '../enums.js';
import { ImageBuilder } from './ImageBuilder.js';

/** Fluent builder for {@link ImageSet} elements. */
export class ImageSetBuilder {
  private readonly imageSet: ImageSet = { type: 'ImageSet', images: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.imageSet.id = id; return this; }
  /** Sets the default display size for all images in the set. @param size The image size. @returns The builder instance for method chaining. */
  withImageSize(size: ImageSize): this { this.imageSet.imageSize = size; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.imageSet.spacing = spacing; return this; }

  /** Adds an image using a builder callback. @param configure A callback to configure the Image builder. @returns The builder instance for method chaining. */
  addImage(configure: (b: ImageBuilder) => void): this;
  /** Adds a pre-built Image object. @param image A pre-built Image. @returns The builder instance for method chaining. */
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

  /** Builds and returns the configured ImageSet. @returns The configured ImageSet instance. */
  build(): ImageSet { return this.imageSet; }
}
