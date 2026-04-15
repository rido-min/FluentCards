import type { ImageSet, Image, AdaptiveElement } from '../models.js';
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
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.imageSet.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.imageSet.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.imageSet.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.imageSet.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.imageSet.requires = { ...this.imageSet.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.imageSet.rtl = rtl; return this; }

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
