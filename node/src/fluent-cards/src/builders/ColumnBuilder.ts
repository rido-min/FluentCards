import type { Column, AdaptiveElement } from '../models.js';
import { ContainerStyle, VerticalAlignment } from '../enums.js';
import { TextBlockBuilder } from './TextBlockBuilder.js';
import { ImageBuilder } from './ImageBuilder.js';
import { ContainerBuilder } from './ContainerBuilder.js';
import { ActionBuilder } from './ActionBuilder.js';
import { BackgroundImageBuilder } from './BackgroundImageBuilder.js';

/** Fluent builder for {@link Column} elements. */
export class ColumnBuilder {
  private readonly column: Column = { type: 'Column', items: [] };

  withId(id: string): this { this.column.id = id; return this; }
  withWidth(width: string): this { this.column.width = width; return this; }
  withStyle(style: ContainerStyle): this { this.column.style = style; return this; }
  withVerticalContentAlignment(alignment: VerticalAlignment): this { this.column.verticalContentAlignment = alignment; return this; }
  withBleed(bleed = true): this { this.column.bleed = bleed; return this; }
  withMinHeight(minHeight: string): this { this.column.minHeight = minHeight; return this; }

  withBackgroundImage(configure: (b: BackgroundImageBuilder) => void): this {
    const b = new BackgroundImageBuilder();
    configure(b);
    this.column.backgroundImage = b.build();
    return this;
  }

  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.column.selectAction = b.build();
    return this;
  }

  addTextBlock(configure: (b: TextBlockBuilder) => void): this {
    const b = new TextBlockBuilder();
    configure(b);
    this.column.items!.push(b.build());
    return this;
  }

  addImage(configure: (b: ImageBuilder) => void): this {
    const b = new ImageBuilder();
    configure(b);
    this.column.items!.push(b.build());
    return this;
  }

  addContainer(configure: (b: ContainerBuilder) => void): this {
    const b = new ContainerBuilder();
    configure(b);
    this.column.items!.push(b.build());
    return this;
  }

  addElement(element: AdaptiveElement): this {
    this.column.items!.push(element);
    return this;
  }

  build(): Column { return this.column; }
}
