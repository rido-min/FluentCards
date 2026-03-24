import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  ActionBuilder,
  AdaptiveCardBuilder,
  toJson,
  ActionStyle,
  AssociatedInputs,
} from 'fluent-cards';
import type { OpenUrlAction, SubmitAction, ShowCardAction, ToggleVisibilityAction, ExecuteAction } from 'fluent-cards';

describe('ActionBuilder', () => {
  it('builds an OpenUrl action', () => {
    const action = new ActionBuilder().openUrl('https://example.com', 'Visit').build() as OpenUrlAction;
    assert.equal(action.type, 'Action.OpenUrl');
    assert.equal(action.url, 'https://example.com');
    assert.equal(action.title, 'Visit');
  });

  it('builds a Submit action', () => {
    const action = new ActionBuilder().submit('Send').build() as SubmitAction;
    assert.equal(action.type, 'Action.Submit');
    assert.equal(action.title, 'Send');
  });

  it('builds a ShowCard action with a nested card', () => {
    const nested = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('Nested'))
      .build();
    const action = new ActionBuilder().showCard('More').withCard(nested).build() as ShowCardAction;
    assert.equal(action.type, 'Action.ShowCard');
    assert.equal(action.title, 'More');
    assert.ok(action.card);
    assert.equal(action.card!.body![0].type, 'TextBlock');
  });

  it('builds a ToggleVisibility action with string targets', () => {
    const action = new ActionBuilder()
      .toggleVisibility('Toggle')
      .addTargetElement('el1')
      .addTargetElement('el2')
      .build() as ToggleVisibilityAction;

    assert.equal(action.type, 'Action.ToggleVisibility');
    assert.deepEqual(action.targetElements, ['el1', 'el2']);
  });

  it('builds a ToggleVisibility action with object targets', () => {
    const action = new ActionBuilder()
      .toggleVisibility()
      .addTargetElement('el1', true)
      .addTargetElement('el2', false)
      .build() as ToggleVisibilityAction;

    assert.deepEqual(action.targetElements, [
      { elementId: 'el1', isVisible: true },
      { elementId: 'el2', isVisible: false },
    ]);
  });

  it('builds an Execute action with verb', () => {
    const action = new ActionBuilder()
      .execute('Run')
      .withVerb('doAction')
      .withData({ key: 'value' })
      .build() as ExecuteAction;

    assert.equal(action.type, 'Action.Execute');
    assert.equal(action.title, 'Run');
    assert.equal(action.verb, 'doAction');
    assert.deepEqual(action.data, { key: 'value' });
  });

  it('sets common properties via modifiers', () => {
    const action = new ActionBuilder()
      .submit()
      .withId('a1')
      .withTitle('Submit')
      .withIconUrl('https://example.com/icon.png')
      .withStyle(ActionStyle.Positive)
      .withIsEnabled(false)
      .withTooltip('Click me')
      .build();

    assert.equal(action.id, 'a1');
    assert.equal(action.title, 'Submit');
    assert.equal(action.iconUrl, 'https://example.com/icon.png');
    assert.equal(action.style, ActionStyle.Positive);
    assert.equal(action.isEnabled, false);
    assert.equal(action.tooltip, 'Click me');
  });

  it('sets associatedInputs on submit', () => {
    const action = new ActionBuilder()
      .submit()
      .withAssociatedInputs(AssociatedInputs.None)
      .build() as SubmitAction;

    assert.equal(action.associatedInputs, AssociatedInputs.None);
  });

  it('throws when no action type is selected', () => {
    assert.throws(() => new ActionBuilder().build(), /No action type specified/);
  });

  it('serializes ShowCard with nested actions', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) =>
        b.showCard('Show').withCard(
          AdaptiveCardBuilder.create()
            .addTextBlock((tb) => tb.withText('Detail'))
            .addAction((a) => a.submit('Submit'))
            .build(),
        ),
      )
      .build();

    const json = toJson(card);
    assert.ok(json.includes('"type": "Action.ShowCard"'));
    assert.ok(json.includes('"type": "Action.Submit"'));
    assert.ok(json.includes('"text": "Detail"'));
  });
});
