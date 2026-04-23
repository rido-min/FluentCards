import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  AuthenticationBuilder,
  RefreshBuilder,
} from 'fluent-cards';
import type {
  ExecuteAction,
  TokenExchangeResource,
  AuthCardButton,
} from 'fluent-cards';

describe('Version and schema URL handling', () => {
  it('withVersion("1.0") maps to 1.0.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.0').build();
    assert.equal(card.version, '1.0');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.0.0/adaptive-card.json');
  });

  it('withVersion("1.1") maps to 1.1.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.1').build();
    assert.equal(card.version, '1.1');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.1.0/adaptive-card.json');
  });

  it('withVersion("1.2") maps to 1.2.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.2').build();
    assert.equal(card.version, '1.2');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.2.0/adaptive-card.json');
  });

  it('withVersion("1.3") maps to 1.3.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.3').build();
    assert.equal(card.version, '1.3');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.3.0/adaptive-card.json');
  });

  it('withVersion("1.4") maps to 1.4.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.4').build();
    assert.equal(card.version, '1.4');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.4.0/adaptive-card.json');
  });

  it('withVersion("1.5") maps to 1.5.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.5').build();
    assert.equal(card.version, '1.5');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.5.0/adaptive-card.json');
  });

  it('withVersion("1.6") maps to 1.6.0 schema', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.6').build();
    assert.equal(card.version, '1.6');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.6.0/adaptive-card.json');
  });

  it('unknown version falls back to generic schema URL', () => {
    const card = AdaptiveCardBuilder.create().withVersion('9.9').build();
    assert.equal(card.version, '9.9');
    assert.equal(card['$schema'], 'http://adaptivecards.io/schemas/adaptive-card.json');
  });

  it('default version is 1.5', () => {
    const card = AdaptiveCardBuilder.create().build();
    assert.equal(card.version, '1.5');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.5.0/adaptive-card.json');
  });

  it('withSchema overrides auto-mapped URL', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.6')
      .withSchema('https://custom.schema/v1')
      .build();
    assert.equal(card.version, '1.6');
    assert.equal(card['$schema'], 'https://custom.schema/v1');
  });

  it('withSchema(undefined) removes schema', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.6')
      .withSchema(undefined)
      .build();
    assert.equal(card['$schema'], undefined);
  });
});

describe('Authentication builder', () => {
  it('builds with text and connectionName', () => {
    const auth = new AuthenticationBuilder()
      .withText('Sign in to continue')
      .withConnectionName('myConnection')
      .build();

    assert.equal(auth.text, 'Sign in to continue');
    assert.equal(auth.connectionName, 'myConnection');
  });

  it('builds with tokenExchangeResource', () => {
    const resource: TokenExchangeResource = {
      id: 'res1',
      uri: 'api://botid/token',
      providerId: 'azure',
    };
    const auth = new AuthenticationBuilder()
      .withTokenExchangeResource(resource)
      .build();

    assert.deepEqual(auth.tokenExchangeResource, resource);
  });

  it('builds with buttons', () => {
    const button1: AuthCardButton = {
      type: 'signin',
      title: 'Sign In',
      image: 'https://example.com/icon.png',
      value: 'https://example.com/auth',
    };
    const button2: AuthCardButton = {
      type: 'openUrl',
      title: 'Register',
      value: 'https://example.com/register',
    };
    const auth = new AuthenticationBuilder()
      .addButton(button1)
      .addButton(button2)
      .build();

    assert.equal(auth.buttons!.length, 2);
    assert.deepEqual(auth.buttons![0], button1);
    assert.deepEqual(auth.buttons![1], button2);
  });

  it('builds with all properties', () => {
    const auth = new AuthenticationBuilder()
      .withText('Auth text')
      .withConnectionName('conn')
      .withTokenExchangeResource({ id: 'r1', uri: 'uri', providerId: 'p' })
      .addButton({ type: 'signin', title: 'Sign In', value: 'url' })
      .build();

    assert.equal(auth.text, 'Auth text');
    assert.equal(auth.connectionName, 'conn');
    assert.ok(auth.tokenExchangeResource);
    assert.equal(auth.buttons!.length, 1);
  });

  it('via card builder withAuthentication', () => {
    const card = AdaptiveCardBuilder.create()
      .withAuthentication((a) =>
        a
          .withText('Please sign in')
          .withConnectionName('OAuth2')
          .addButton({ type: 'signin', title: 'SSO', value: 'https://sso.example.com' }),
      )
      .build();

    assert.ok(card.authentication);
    assert.equal(card.authentication!.text, 'Please sign in');
    assert.equal(card.authentication!.connectionName, 'OAuth2');
    assert.equal(card.authentication!.buttons!.length, 1);
  });
});

describe('Refresh builder', () => {
  it('builds with action and userIds', () => {
    const refresh = new RefreshBuilder()
      .withAction((a) => a.execute('Refresh').withVerb('refresh'))
      .addUserId('user1')
      .addUserId('user2')
      .build();

    assert.ok(refresh.action);
    assert.equal(refresh.action!.type, 'Action.Execute');
    assert.equal((refresh.action as ExecuteAction).verb, 'refresh');
    assert.deepEqual(refresh.userIds, ['user1', 'user2']);
  });

  it('builds with expires', () => {
    const refresh = new RefreshBuilder()
      .withAction((a) => a.execute('Refresh'))
      .withExpires('2024-12-31T23:59:59Z')
      .build();

    assert.equal(refresh.expires, '2024-12-31T23:59:59Z');
  });

  it('via card builder withRefresh', () => {
    const card = AdaptiveCardBuilder.create()
      .withRefresh((r) =>
        r
          .withAction((a) => a.execute('Auto Refresh').withVerb('autoRefresh'))
          .addUserId('user-abc')
          .withExpires('2025-01-01T00:00:00Z'),
      )
      .build();

    assert.ok(card.refresh);
    assert.ok(card.refresh!.action);
    assert.equal(card.refresh!.action!.type, 'Action.Execute');
    assert.deepEqual(card.refresh!.userIds, ['user-abc']);
    assert.equal(card.refresh!.expires, '2025-01-01T00:00:00Z');
  });
});

describe('Metadata', () => {
  it('sets webUrl via withMetadata', () => {
    const card = AdaptiveCardBuilder.create()
      .withMetadata('https://example.com/deep-link')
      .build();

    assert.deepEqual(card.metadata, { webUrl: 'https://example.com/deep-link' });
  });

  it('metadata is undefined when not set', () => {
    const card = AdaptiveCardBuilder.create().build();
    assert.equal(card.metadata, undefined);
  });
});
