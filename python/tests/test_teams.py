import pytest
from fluent_cards import TeamsAdaptiveCards, to_json, validate, ValidationSeverity


class TestTeamsAdaptiveCardsApprovalCard:
    def test_returns_valid_adaptive_card(self):
        card = TeamsAdaptiveCards.create_approval_card(
            requester_name='Jane Doe',
            submitted_date='2024-04-01',
            title='Q1 Budget Approval',
            category='Finance',
            amount='$5,000',
            business_unit='Engineering',
            due_date='2024-04-15',
            description='Please approve the Q1 budget.',
        )
        assert card['type'] == 'AdaptiveCard'
        assert card['version'] == '1.5'
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0, f'Unexpected errors: {errors}'

    def test_includes_approve_and_decline_actions(self):
        card = TeamsAdaptiveCards.create_approval_card(
            requester_name='Jane',
            submitted_date='2024-04-01',
            title='T',
            category='C',
            amount='$1',
            business_unit='B',
            due_date='2024-04-10',
            description='D',
        )
        json_str = to_json(card)
        assert '"Approve"' in json_str
        assert '"Decline"' in json_str

    def test_with_requester_image_url(self):
        card = TeamsAdaptiveCards.create_approval_card(
            requester_name='Jane',
            submitted_date='2024-04-01',
            title='T',
            category='C',
            amount='$1',
            business_unit='B',
            due_date='2024-04-10',
            description='D',
            requester_image_url='https://example.com/avatar.png',
        )
        json_str = to_json(card)
        assert 'https://example.com/avatar.png' in json_str
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0


class TestTeamsAdaptiveCardsStatusUpdateCard:
    def test_returns_valid_adaptive_card(self):
        card = TeamsAdaptiveCards.create_status_update_card(
            card_title='Sprint 12 Status',
            team_name='Platform',
            update_date='2024-04-01',
            project='FluentCards',
            status='On Track',
            sprint='Sprint 12',
            completion='75%',
            updated_by='John',
            notes='Going well.',
            project_url='https://example.com/project',
        )
        assert card['type'] == 'AdaptiveCard'
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0


class TestTeamsAdaptiveCardsTaskUpdateCard:
    def test_returns_valid_adaptive_card(self):
        card = TeamsAdaptiveCards.create_task_update_card(
            task_name='Implement feature',
            project='FluentCards',
            assigned_by='Alice',
            due_date='2024-04-20',
            estimate='3 days',
            priority='High',
            description='Build the feature.',
            task_url='https://example.com/task',
        )
        assert card['type'] == 'AdaptiveCard'
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0


class TestTeamsAdaptiveCardsMeetingReminderCard:
    def test_returns_valid_adaptive_card(self):
        card = TeamsAdaptiveCards.create_meeting_reminder_card(
            meeting_title='Team Sync',
            organizer='Bob',
            date='2024-04-05',
            time='10:00 AM',
            location='Teams',
            attendees='Alice, Bob, Carol',
            agenda='Weekly sync.',
            join_url='https://teams.microsoft.com/meeting',
            details_url='https://outlook.com/event',
        )
        assert card['type'] == 'AdaptiveCard'
        json_str = to_json(card)
        assert 'Join Meeting' in json_str
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0


class TestTeamsAdaptiveCardsExpenseReportCard:
    def test_returns_valid_adaptive_card(self):
        card = TeamsAdaptiveCards.create_expense_report_card(
            employee_name='Carol',
            employee_job_title='Engineer',
            report_id='EXP-001',
            submitted_date='2024-04-01',
            category='Travel',
            total_amount='$1,200',
            currency='USD',
            description='Travel to conference.',
            report_url='https://example.com/report',
        )
        assert card['type'] == 'AdaptiveCard'
        json_str = to_json(card)
        assert '"Approve"' in json_str
        assert '"Reject"' in json_str
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0

    def test_with_employee_image_url(self):
        card = TeamsAdaptiveCards.create_expense_report_card(
            employee_name='Carol',
            employee_job_title='Engineer',
            report_id='EXP-001',
            submitted_date='2024-04-01',
            category='Travel',
            total_amount='$1,200',
            currency='USD',
            description='Travel to conference.',
            report_url='https://example.com/report',
            employee_image_url='https://example.com/employee.png',
        )
        json_str = to_json(card)
        assert 'https://example.com/employee.png' in json_str
        errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
        assert len(errors) == 0
