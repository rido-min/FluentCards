import { Spacing } from '../enums.js';
import type { Media, MediaSource } from '../models.js';

/** Fluent builder for {@link Media} elements. */
export class MediaBuilder {
  private readonly media: Media = { type: 'Media', sources: [] };

  withId(id: string): this { this.media.id = id; return this; }
  withPoster(poster: string): this { this.media.poster = poster; return this; }
  withAltText(altText: string): this { this.media.altText = altText; return this; }
  withSpacing(spacing: Spacing): this { this.media.spacing = spacing; return this; }

  addSource(url: string, mimeType: string): this;
  addSource(source: MediaSource): this;
  addSource(urlOrSource: string | MediaSource, mimeType?: string): this {
    if (typeof urlOrSource === 'string') {
      this.media.sources!.push({ url: urlOrSource, mimeType });
    } else {
      this.media.sources!.push(urlOrSource);
    }
    return this;
  }

  build(): Media { return this.media; }
}
