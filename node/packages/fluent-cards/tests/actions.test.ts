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

  it('throws when calling withId before setting action type', () => {
    assert.throws(() => new ActionBuilder().withId('a1'), /No action type has been specified/);
  });

  it('throws when calling withTitle before setting action type', () => {
    assert.throws(() => new ActionBuilder().withTitle('T'), /No action type has been specified/);
  });

  it('throws when calling withIconUrl before setting action type', () => {
    assert.throws(() => new ActionBuilder().withIconUrl('https://x.com/icon.png'), /No action type has been specified/);
  });

  it('throws when calling withStyle before setting action type', () => {
    assert.throws(() => new ActionBuilder().withStyle(ActionStyle.Positive), /No action type has been specified/);
  });

  it('throws when calling withIsEnabled before setting action type', () => {
    assert.throws(() => new ActionBuilder().withIsEnabled(false), /No action type has been specified/);
  });

  it('throws when calling withTooltip before setting action type', () => {
    assert.throws(() => new ActionBuilder().withTooltip('tip'), /No action type has been specified/);
  });

  it('throws when calling withData before setting action type', () => {
    assert.throws(() => new ActionBuilder().withData({ x: 1 }), /No action type has been specified/);
  });

  it('throws when calling addTargetElement before setting action type', () => {
    assert.throws(() => new ActionBuilder().addTargetElement('el1'), /No action type has been specified/);
  });

  // ─── Issue #66: withData() throws on incompatible action types ─────────

  it('withData() throws on OpenUrl action', () => {
    assert.throws(
      () => new ActionBuilder().openUrl('https://example.com').withData({ x: 1 }),
      /withData\(\) is only available on Submit or Execute actions/,
    );
  });

  it('withData() throws on ShowCard action', () => {
    assert.throws(
      () => new ActionBuilder().showCard('Show').withData({ x: 1 }),
      /withData\(\) is only available on Submit or Execute actions/,
    );
  });

  it('withData() throws on ToggleVisibility action', () => {
    assert.throws(
      () => new ActionBuilder().toggleVisibility('Toggle').withData({ x: 1 }),
      /withData\(\) is only available on Submit or Execute actions/,
    );
  });

  it('withData() works on Submit action', () => {
    const action = new ActionBuilder().submit().withData({ key: 'val' }).build() as SubmitAction;
    assert.deepEqual(action.data, { key: 'val' });
  });

  it('withData() works on Execute action', () => {
    const action = new ActionBuilder().execute().withData({ key: 'val' }).build() as ExecuteAction;
    assert.deepEqual(action.data, { key: 'val' });
  });

  // ─── Issue #66: withVerb() throws on incompatible action types ────────

  it('withVerb() throws on OpenUrl action', () => {
    assert.throws(
      () => new ActionBuilder().openUrl('https://example.com').withVerb('doIt'),
      /withVerb\(\) is only available on Execute actions/,
    );
  });

  it('withVerb() throws on Submit action', () => {
    assert.throws(
      () => new ActionBuilder().submit().withVerb('doIt'),
      /withVerb\(\) is only available on Execute actions/,
    );
  });

  it('withVerb() throws on ShowCard action', () => {
    assert.throws(
      () => new ActionBuilder().showCard().withVerb('doIt'),
      /withVerb\(\) is only available on Execute actions/,
    );
  });

  it('withVerb() throws on ToggleVisibility action', () => {
    assert.throws(
      () => new ActionBuilder().toggleVisibility().withVerb('doIt'),
      /withVerb\(\) is only available on Execute actions/,
    );
  });

  it('withVerb() works on Execute action', () => {
    const action = new ActionBuilder().execute().withVerb('doIt').build() as ExecuteAction;
    assert.equal(action.verb, 'doIt');
  });

  // ─── Issue #66: withAssociatedInputs() throws on incompatible types ───

  it('withAssociatedInputs() throws on OpenUrl action', () => {
    assert.throws(
      () => new ActionBuilder().openUrl('https://example.com').withAssociatedInputs(AssociatedInputs.Auto),
      /withAssociatedInputs\(\) is only available on Submit or Execute actions/,
    );
  });

  it('withAssociatedInputs() throws on ShowCard action', () => {
    assert.throws(
      () => new ActionBuilder().showCard().withAssociatedInputs(AssociatedInputs.Auto),
      /withAssociatedInputs\(\) is only available on Submit or Execute actions/,
    );
  });

  it('withAssociatedInputs() throws on ToggleVisibility action', () => {
    assert.throws(
      () => new ActionBuilder().toggleVisibility().withAssociatedInputs(AssociatedInputs.Auto),
      /withAssociatedInputs\(\) is only available on Submit or Execute actions/,
    );
  });

  it('withAssociatedInputs() works on Submit action', () => {
    const action = new ActionBuilder().submit().withAssociatedInputs(AssociatedInputs.None).build() as SubmitAction;
    assert.equal(action.associatedInputs, AssociatedInputs.None);
  });

  it('withAssociatedInputs() works on Execute action', () => {
    const action = new ActionBuilder().execute().withAssociatedInputs(AssociatedInputs.Auto).build() as ExecuteAction;
    assert.equal(action.associatedInputs, AssociatedInputs.Auto);
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
