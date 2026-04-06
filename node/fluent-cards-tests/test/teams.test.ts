import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import { TeamsAdaptiveCards, toJson, validate, ValidationSeverity } from 'fluent-cards';

describe('TeamsAdaptiveCards', () => {
  describe('createApprovalCard', () => {
    it('returns a valid AdaptiveCard', () => {
      const card = TeamsAdaptiveCards.createApprovalCard({
        requesterName: 'Jane Doe',
        submittedDate: '2024-04-01',
        title: 'Q1 Budget Approval',
        category: 'Finance',
        amount: '$5,000',
        businessUnit: 'Engineering',
        dueDate: '2024-04-15',
        description: 'Please approve the Q1 budget.',
      });
      assert.equal(card.type, 'AdaptiveCard');
      assert.equal(card.version, '1.5');
      const issues = validate(card).filter((i) => i.severity === ValidationSeverity.Error);
      assert.equal(issues.length, 0, `Unexpected errors: ${JSON.stringify(issues)}`);
    });

    it('includes Approve and Decline actions', () => {
      const card = TeamsAdaptiveCards.createApprovalCard({
        requesterName: 'Jane', submittedDate: '2024-04-01', title: 'T',
        category: 'C', amount: '$1', businessUnit: 'B', dueDate: '2024-04-10',
        description: 'D',
      });
      const json = toJson(card);
      assert.ok(json.includes('"Approve"'));
      assert.ok(json.includes('"Decline"'));
    });
  });

  describe('createStatusUpdateCard', () => {
    it('returns a valid AdaptiveCard', () => {
      const card = TeamsAdaptiveCards.createStatusUpdateCard({
        cardTitle: 'Sprint 12 Status', teamName: 'Platform', updateDate: '2024-04-01',
        project: 'FluentCards', status: 'On Track', sprint: 'Sprint 12',
        completion: '75%', updatedBy: 'John', notes: 'Going well.',
        projectUrl: 'https://example.com/project',
      });
      assert.equal(card.type, 'AdaptiveCard');
      const issues = validate(card).filter((i) => i.severity === ValidationSeverity.Error);
      assert.equal(issues.length, 0);
    });
  });

  describe('createTaskUpdateCard', () => {
    it('returns a valid AdaptiveCard', () => {
      const card = TeamsAdaptiveCards.createTaskUpdateCard({
        taskName: 'Implement feature', project: 'FluentCards', assignedBy: 'Alice',
        dueDate: '2024-04-20', estimate: '3 days', priority: 'High',
        description: 'Build the feature.', taskUrl: 'https://example.com/task',
      });
      assert.equal(card.type, 'AdaptiveCard');
      const issues = validate(card).filter((i) => i.severity === ValidationSeverity.Error);
      assert.equal(issues.length, 0);
    });
  });

  describe('createMeetingReminderCard', () => {
    it('returns a valid AdaptiveCard', () => {
      const card = TeamsAdaptiveCards.createMeetingReminderCard({
        meetingTitle: 'Team Sync', organizer: 'Bob', date: '2024-04-05',
        time: '10:00 AM', location: 'Teams', attendees: 'Alice, Bob, Carol',
        agenda: 'Weekly sync.', joinUrl: 'https://teams.microsoft.com/meeting',
        detailsUrl: 'https://outlook.com/event',
      });
      assert.equal(card.type, 'AdaptiveCard');
      const json = toJson(card);
      assert.ok(json.includes('Join Meeting'));
      const issues = validate(card).filter((i) => i.severity === ValidationSeverity.Error);
      assert.equal(issues.length, 0);
    });
  });

  describe('createExpenseReportCard', () => {
    it('returns a valid AdaptiveCard', () => {
      const card = TeamsAdaptiveCards.createExpenseReportCard({
        employeeName: 'Carol', employeeJobTitle: 'Engineer', reportId: 'EXP-001',
        submittedDate: '2024-04-01', category: 'Travel', totalAmount: '$1,200',
        currency: 'USD', description: 'Travel to conference.',
        reportUrl: 'https://example.com/report',
      });
      assert.equal(card.type, 'AdaptiveCard');
      const json = toJson(card);
      assert.ok(json.includes('"Approve"'));
      assert.ok(json.includes('"Reject"'));
      const issues = validate(card).filter((i) => i.severity === ValidationSeverity.Error);
      assert.equal(issues.length, 0);
    });
  });
});
