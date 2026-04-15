import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  ActionBuilder,
  TeamsDataBuilder,
  toJson,
  validate,
  ValidationSeverity,
} from 'fluent-cards';

describe('Teams Features', () => {
  // ── Model & Serialization Tests ──

  describe('Serialization', () => {
    it('serializes width Full', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.withFullWidth())
        .addTextBlock(tb => tb.withText('Hello'))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.msteams.width, 'Full');
    });

    it('omits width when not set', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('John', '29:123'))
        .addTextBlock(tb => tb.withText('<at>John</at>'))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.msteams.width, undefined);
    });

    it('omits msteams when not set', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Hello'))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.msteams, undefined);
    });

    it('serializes mentions', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('John Doe', '29:1241241'))
        .addTextBlock(tb => tb.withText('Hello <at>John Doe</at>!'))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.msteams.entities.length, 1);
      assert.equal(json.msteams.entities[0].type, 'mention');
      assert.equal(json.msteams.entities[0].text, '<at>John Doe</at>');
      assert.equal(json.msteams.entities[0].mentioned.id, '29:1241241');
      assert.equal(json.msteams.entities[0].mentioned.name, 'John Doe');
    });

    it('serializes submit feedback', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Test'))
        .addAction(a => a
          .submit('Silent Submit')
          .withTeamsSubmitFeedback(ts => ts.withFeedbackHidden()))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.actions[0].msteams.feedback.hide, true);
    });

    it('serializes msteams and custom properties in data', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Test'))
        .addAction(a => a
          .submit('Task Module')
          .withTeamsData(td => td
            .withTaskFetch()
            .withProperty('customField', 'customValue')))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.actions[0].data.msteams.type, 'task/fetch');
      assert.equal(json.actions[0].data.customField, 'customValue');
    });

    it('serializes msteams only in data', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Test'))
        .addAction(a => a.submit('Module').withTeamsTaskFetch())
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.actions[0].data.msteams.type, 'task/fetch');
      assert.deepEqual(Object.keys(json.actions[0].data), ['msteams']);
    });

    it('serializes custom properties only in data', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Test'))
        .addAction(a => a
          .submit('Feedback')
          .withTeamsData(td => td
            .withProperty('message', 'Clicked')
            .withProperty('source', 'card')))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.actions[0].data.message, 'Clicked');
      assert.equal(json.actions[0].data.source, 'card');
      assert.equal(json.actions[0].data.msteams, undefined);
    });
  });

  // ── Builder Tests ──

  describe('Builders', () => {
    it('withTeamsCard sets width', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.withFullWidth())
        .addTextBlock(tb => tb.withText('Test'))
        .build();
      assert.equal(card.msteams?.width, 'Full');
    });

    it('addMention auto-generates at text', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('John', '29:123'))
        .addTextBlock(tb => tb.withText('<at>John</at>'))
        .build();
      assert.equal(card.msteams?.entities?.length, 1);
      assert.equal(card.msteams!.entities![0].text, '<at>John</at>');
      assert.equal(card.msteams!.entities![0].mentioned.id, '29:123');
    });

    it('supports multiple mentions', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('Alice', '29:111').addMention('Bob', '29:222'))
        .addTextBlock(tb => tb.withText('<at>Alice</at> and <at>Bob</at>'))
        .build();
      assert.equal(card.msteams!.entities!.length, 2);
    });

    it('withTeamsData with all property types', () => {
      const b = new TeamsDataBuilder();
      b.withProperty('str', 'hello');
      b.withProperty('num', 42);
      b.withProperty('bool', true);
      b.withProperty('obj', { nested: 'value' });
      const result = b.build();
      assert.equal(result.str, 'hello');
      assert.equal(result.num, 42);
      assert.equal(result.bool, true);
      assert.deepEqual(result.obj, { nested: 'value' });
    });
  });

  // ── Submit-Only Gating Tests ──

  describe('Submit-only gating', () => {
    it('withTeamsTaskFetch on execute throws', () => {
      const b = new ActionBuilder();
      b.execute('Test');
      assert.throws(() => b.withTeamsTaskFetch(), /Submit/);
    });

    it('withTeamsData on execute throws', () => {
      const b = new ActionBuilder();
      b.execute('Test');
      assert.throws(() => b.withTeamsData(td => td.withTaskFetch()), /Submit/);
    });

    it('withTeamsSubmitFeedback on execute throws', () => {
      const b = new ActionBuilder();
      b.execute('Test');
      assert.throws(() => b.withTeamsSubmitFeedback(ts => ts.withFeedbackHidden()), /Submit/);
    });

    it('withTeamsTaskFetch on openUrl throws', () => {
      const b = new ActionBuilder();
      b.openUrl('https://example.com');
      assert.throws(() => b.withTeamsTaskFetch(), /Submit/);
    });
  });

  // ── Conflict Detection Tests ──

  describe('Conflict detection', () => {
    it('withTeamsData then withData throws', () => {
      const b = new ActionBuilder();
      b.submit('Test');
      b.withTeamsData(td => td.withTaskFetch());
      assert.throws(() => b.withData({ key: 'value' }), /withData/);
    });

    it('withData then withTeamsTaskFetch throws', () => {
      const b = new ActionBuilder();
      b.submit('Test');
      b.withData({ key: 'value' });
      assert.throws(() => b.withTeamsTaskFetch(), /withData/);
    });

    it('withTeamsCard then withTeamsCardRaw throws', () => {
      const b = AdaptiveCardBuilder.create();
      b.withTeamsCard(t => t.withFullWidth());
      assert.throws(() => b.withTeamsCardRaw({ width: 'Full' }), /withTeamsCard/);
    });

    it('withTeamsCardRaw then withTeamsCard throws', () => {
      const b = AdaptiveCardBuilder.create();
      b.withTeamsCardRaw({ width: 'Full' });
      assert.throws(() => b.withTeamsCard(t => t.withFullWidth()), /withTeamsCard/);
    });

    it('withTeamsSubmitFeedback then withTeamsSubmitRaw throws', () => {
      const b = new ActionBuilder();
      b.submit('Test');
      b.withTeamsSubmitFeedback(ts => ts.withFeedbackHidden());
      assert.throws(() => b.withTeamsSubmitRaw({ feedback: { hide: true } }), /withTeamsSubmitFeedback/);
    });

    it('TeamsDataBuilder rejects msteams key', () => {
      const b = new TeamsDataBuilder();
      assert.throws(() => b.withProperty('msteams', 'value'), /msteams/);
    });

    it('TeamsDataBuilder rejects msteams key case-insensitive', () => {
      const b = new TeamsDataBuilder();
      assert.throws(() => b.withProperty('MsTeams', 'value'), /msteams/);
    });
  });

  // ── Validation Tests ──

  describe('Validation', () => {
    it('warns on orphaned mention entity', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('John', '29:123'))
        .addTextBlock(tb => tb.withText('Hello world'))
        .build();
      const issues = validate(card);
      assert.ok(issues.some(i => i.code === 'ORPHANED_MENTION_ENTITY'));
    });

    it('warns on orphaned at-token', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('Alice', '29:456'))
        .addTextBlock(tb => tb.withText('Hello <at>John</at>'))
        .build();
      const issues = validate(card);
      assert.ok(issues.some(i => i.code === 'ORPHANED_AT_TOKEN'));
    });

    it('passes matched mentions', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCard(t => t.addMention('John', '29:123').addMention('Alice', '29:456'))
        .addTextBlock(tb => tb.withText('Hello <at>John</at> and <at>Alice</at>'))
        .build();
      const issues = validate(card);
      assert.ok(!issues.some(i => i.code === 'ORPHANED_MENTION_ENTITY'));
      assert.ok(!issues.some(i => i.code === 'ORPHANED_AT_TOKEN'));
    });
  });

  // ── Integration Tests ──

  describe('Integration', () => {
    it('full Teams card matches expected shape', () => {
      const card = AdaptiveCardBuilder.create()
        .withVersion('1.5')
        .withTeamsCard(teams => teams
          .withFullWidth()
          .addMention('John Doe', '29:1241241'))
        .addTextBlock(t => t.withText('Hello <at>John Doe</at>!'))
        .addAction(a => a.submit('Open Task Module').withTeamsTaskFetch())
        .addAction(a => a.submit('Silent').withTeamsSubmitFeedback(ts => ts.withFeedbackHidden()))
        .build();

      const json = JSON.parse(toJson(card));
      assert.equal(json.msteams.width, 'Full');
      assert.equal(json.msteams.entities.length, 1);
      assert.equal(json.actions[0].data.msteams.type, 'task/fetch');
      assert.equal(json.actions[1].msteams.feedback.hide, true);
    });

    it('existing cards unaffected', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Hello'))
        .addAction(a => a.submit('OK').withData({ key: 'value' }))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.msteams, undefined);
      assert.equal(json.actions[0].type, 'Action.Submit');
    });

    it('withTeamsCardRaw passes through', () => {
      const card = AdaptiveCardBuilder.create()
        .withTeamsCardRaw({ width: 'Full' })
        .addTextBlock(tb => tb.withText('Test'))
        .build();
      assert.equal(card.msteams?.width, 'Full');
    });

    it('withTeamsSubmitRaw serializes correctly', () => {
      const card = AdaptiveCardBuilder.create()
        .addTextBlock(tb => tb.withText('Test'))
        .addAction(a => a
          .submit('Silent')
          .withTeamsSubmitRaw({ feedback: { hide: true } }))
        .build();
      const json = JSON.parse(toJson(card));
      assert.equal(json.actions[0].msteams.feedback.hide, true);
    });
  });
});
