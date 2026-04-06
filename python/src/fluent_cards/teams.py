from __future__ import annotations
from typing import Optional
from .enums import ActionStyle, ContainerStyle, ImageSize, ImageStyle, TextColor, TextSize, TextWeight, VerticalAlignment
from .builders.adaptive_card_builder import AdaptiveCardBuilder
from .builders.column_set_builder import ColumnSetBuilder


def create_approval_card(
    requester_name: str,
    submitted_date: str,
    title: str,
    category: str,
    amount: str,
    business_unit: str,
    due_date: str,
    description: str,
    requester_image_url: Optional[str] = None,
) -> dict:
    """
    Creates an approval request card with Approve and Decline actions.
    Reflects the Teams approval sample pattern where a requester asks for sign-off on an item.
    """
    b = AdaptiveCardBuilder.create().with_version('1.5')

    def build_cs(cs):
        if requester_image_url:
            cs.add_column('auto', lambda col: col
                          .add_image(lambda img: img
                                     .with_url(requester_image_url)
                                     .with_size(ImageSize.Small)
                                     .with_style(ImageStyle.Person)))
        cs.add_column('stretch', lambda col: col
                      .with_vertical_content_alignment(VerticalAlignment.Center)
                      .add_text_block(lambda tb: tb
                                      .with_text(requester_name)
                                      .with_weight(TextWeight.Bolder)
                                      .with_wrap(True))
                      .add_text_block(lambda tb: tb
                                      .with_text(submitted_date)
                                      .with_is_subtle()
                                      .with_size(TextSize.Small)
                                      .with_wrap(True)))

    b.add_column_set(build_cs)
    return (b
            .add_text_block(lambda tb: tb
                            .with_text(title)
                            .with_size(TextSize.Large)
                            .with_weight(TextWeight.Bolder)
                            .with_wrap(True))
            .add_fact_set(lambda fs: fs
                          .add_fact('Category', category)
                          .add_fact('Amount', amount)
                          .add_fact('Business Unit', business_unit)
                          .add_fact('Due Date', due_date))
            .add_text_block(lambda tb: tb
                            .with_text(description)
                            .with_wrap(True)
                            .with_is_subtle())
            .add_action(lambda a: a
                        .submit('Approve')
                        .with_style(ActionStyle.Positive))
            .add_action(lambda a: a
                        .submit('Decline')
                        .with_style(ActionStyle.Destructive))
            .build())


def create_status_update_card(
    card_title: str,
    team_name: str,
    update_date: str,
    project: str,
    status: str,
    sprint: str,
    completion: str,
    updated_by: str,
    notes: str,
    project_url: str,
) -> dict:
    """
    Creates a status update notification card showing the current state of a project or task.
    Reflects the Teams status-update sample pattern used in project tracking scenarios.
    """
    return (AdaptiveCardBuilder.create()
            .with_version('1.5')
            .add_container(lambda c: c
                           .with_style(ContainerStyle.Emphasis)
                           .add_column_set(lambda cs: cs
                                           .add_column('stretch', lambda col: col
                                                       .add_text_block(lambda tb: tb
                                                                       .with_text(card_title)
                                                                       .with_size(TextSize.Large)
                                                                       .with_weight(TextWeight.Bolder)
                                                                       .with_wrap(True))
                                                       .add_text_block(lambda tb: tb
                                                                       .with_text(f'{team_name} \u2022 {update_date}')
                                                                       .with_is_subtle()
                                                                       .with_size(TextSize.Small)
                                                                       .with_wrap(True)))))
            .add_fact_set(lambda fs: fs
                          .add_fact('Project', project)
                          .add_fact('Status', status)
                          .add_fact('Sprint', sprint)
                          .add_fact('Completion', completion)
                          .add_fact('Updated By', updated_by))
            .add_text_block(lambda tb: tb
                            .with_text(notes)
                            .with_wrap(True))
            .add_action(lambda a: a
                        .open_url(project_url)
                        .with_title('View Project'))
            .build())


def create_task_update_card(
    task_name: str,
    project: str,
    assigned_by: str,
    due_date: str,
    estimate: str,
    priority: str,
    description: str,
    task_url: str,
) -> dict:
    """
    Creates a task assignment notification card informing the recipient of a newly assigned task.
    Reflects the Teams task-update sample pattern used in work tracking integrations.
    """
    return (AdaptiveCardBuilder.create()
            .with_version('1.5')
            .add_column_set(lambda cs: cs
                            .add_column('stretch', lambda col: col
                                        .add_text_block(lambda tb: tb
                                                        .with_text('Task Assigned to You')
                                                        .with_size(TextSize.Large)
                                                        .with_weight(TextWeight.Bolder)
                                                        .with_wrap(True)))
                            .add_column('auto', lambda col: col
                                        .with_vertical_content_alignment(VerticalAlignment.Center)
                                        .add_text_block(lambda tb: tb
                                                        .with_text(priority)
                                                        .with_color(TextColor.Attention)
                                                        .with_weight(TextWeight.Bolder))))
            .add_fact_set(lambda fs: fs
                          .add_fact('Task', task_name)
                          .add_fact('Project', project)
                          .add_fact('Assigned By', assigned_by)
                          .add_fact('Due Date', due_date)
                          .add_fact('Estimate', estimate))
            .add_text_block(lambda tb: tb
                            .with_text(description)
                            .with_wrap(True)
                            .with_is_subtle())
            .add_action(lambda a: a
                        .open_url(task_url)
                        .with_title('View Task'))
            .add_action(lambda a: a
                        .submit('Acknowledge')
                        .with_style(ActionStyle.Positive))
            .build())


def create_meeting_reminder_card(
    meeting_title: str,
    organizer: str,
    date: str,
    time: str,
    location: str,
    attendees: str,
    agenda: str,
    join_url: str,
    details_url: str,
) -> dict:
    """
    Creates a meeting reminder card with meeting details and a join link.
    Reflects the Teams meeting-invite sample pattern used in calendar integration scenarios.
    """
    return (AdaptiveCardBuilder.create()
            .with_version('1.5')
            .add_text_block(lambda tb: tb
                            .with_text('\u23f0 Meeting Starting Soon')
                            .with_size(TextSize.Large)
                            .with_weight(TextWeight.Bolder)
                            .with_wrap(True))
            .add_text_block(lambda tb: tb
                            .with_text(meeting_title)
                            .with_size(TextSize.Medium)
                            .with_wrap(True))
            .add_fact_set(lambda fs: fs
                          .add_fact('Organizer', organizer)
                          .add_fact('Date', date)
                          .add_fact('Time', time)
                          .add_fact('Location', location)
                          .add_fact('Attendees', attendees))
            .add_text_block(lambda tb: tb
                            .with_text(agenda)
                            .with_wrap(True)
                            .with_is_subtle())
            .add_action(lambda a: a
                        .open_url(join_url)
                        .with_title('Join Meeting')
                        .with_style(ActionStyle.Positive))
            .add_action(lambda a: a
                        .open_url(details_url)
                        .with_title('View Details'))
            .build())


def create_expense_report_card(
    employee_name: str,
    employee_job_title: str,
    report_id: str,
    submitted_date: str,
    category: str,
    total_amount: str,
    currency: str,
    description: str,
    report_url: str,
    employee_image_url: Optional[str] = None,
) -> dict:
    """
    Creates an expense report card for finance team review with Approve and Reject actions.
    Reflects the Teams expense-report sample pattern used in finance approval workflows.
    """
    b = (AdaptiveCardBuilder.create()
         .with_version('1.5')
         .add_container(lambda c: c
                        .with_style(ContainerStyle.Emphasis)
                        .add_text_block(lambda tb: tb
                                        .with_text('Expense Report Submitted')
                                        .with_size(TextSize.Large)
                                        .with_weight(TextWeight.Bolder)
                                        .with_wrap(True))
                        .add_text_block(lambda tb: tb
                                        .with_text('Awaiting your review and approval')
                                        .with_is_subtle()
                                        .with_wrap(True))))

    def build_cs(cs):
        if employee_image_url:
            cs.add_column('auto', lambda col: col
                          .add_image(lambda img: img
                                     .with_url(employee_image_url)
                                     .with_size(ImageSize.Small)
                                     .with_style(ImageStyle.Person)))
        cs.add_column('stretch', lambda col: col
                      .with_vertical_content_alignment(VerticalAlignment.Center)
                      .add_text_block(lambda tb: tb
                                      .with_text(employee_name)
                                      .with_weight(TextWeight.Bolder)
                                      .with_wrap(True))
                      .add_text_block(lambda tb: tb
                                      .with_text(employee_job_title)
                                      .with_is_subtle()
                                      .with_size(TextSize.Small)
                                      .with_wrap(True)))

    b.add_column_set(build_cs)
    return (b
            .add_fact_set(lambda fs: fs
                          .add_fact('Report ID', report_id)
                          .add_fact('Submitted', submitted_date)
                          .add_fact('Category', category)
                          .add_fact('Total Amount', total_amount)
                          .add_fact('Currency', currency))
            .add_text_block(lambda tb: tb
                            .with_text(description)
                            .with_wrap(True)
                            .with_is_subtle())
            .add_action(lambda a: a
                        .submit('Approve')
                        .with_style(ActionStyle.Positive))
            .add_action(lambda a: a
                        .submit('Reject')
                        .with_style(ActionStyle.Destructive))
            .add_action(lambda a: a
                        .open_url(report_url)
                        .with_title('View Report'))
            .build())


class TeamsAdaptiveCards:
    """
    Provides helper methods for creating Microsoft Teams-style Adaptive Cards,
    reflecting common patterns from the Teams Adaptive Card Samples collection.
    """

    @staticmethod
    def create_approval_card(
        requester_name: str,
        submitted_date: str,
        title: str,
        category: str,
        amount: str,
        business_unit: str,
        due_date: str,
        description: str,
        requester_image_url: Optional[str] = None,
    ) -> dict:
        return create_approval_card(
            requester_name=requester_name,
            submitted_date=submitted_date,
            title=title,
            category=category,
            amount=amount,
            business_unit=business_unit,
            due_date=due_date,
            description=description,
            requester_image_url=requester_image_url,
        )

    @staticmethod
    def create_status_update_card(
        card_title: str,
        team_name: str,
        update_date: str,
        project: str,
        status: str,
        sprint: str,
        completion: str,
        updated_by: str,
        notes: str,
        project_url: str,
    ) -> dict:
        return create_status_update_card(
            card_title=card_title,
            team_name=team_name,
            update_date=update_date,
            project=project,
            status=status,
            sprint=sprint,
            completion=completion,
            updated_by=updated_by,
            notes=notes,
            project_url=project_url,
        )

    @staticmethod
    def create_task_update_card(
        task_name: str,
        project: str,
        assigned_by: str,
        due_date: str,
        estimate: str,
        priority: str,
        description: str,
        task_url: str,
    ) -> dict:
        return create_task_update_card(
            task_name=task_name,
            project=project,
            assigned_by=assigned_by,
            due_date=due_date,
            estimate=estimate,
            priority=priority,
            description=description,
            task_url=task_url,
        )

    @staticmethod
    def create_meeting_reminder_card(
        meeting_title: str,
        organizer: str,
        date: str,
        time: str,
        location: str,
        attendees: str,
        agenda: str,
        join_url: str,
        details_url: str,
    ) -> dict:
        return create_meeting_reminder_card(
            meeting_title=meeting_title,
            organizer=organizer,
            date=date,
            time=time,
            location=location,
            attendees=attendees,
            agenda=agenda,
            join_url=join_url,
            details_url=details_url,
        )

    @staticmethod
    def create_expense_report_card(
        employee_name: str,
        employee_job_title: str,
        report_id: str,
        submitted_date: str,
        category: str,
        total_amount: str,
        currency: str,
        description: str,
        report_url: str,
        employee_image_url: Optional[str] = None,
    ) -> dict:
        return create_expense_report_card(
            employee_name=employee_name,
            employee_job_title=employee_job_title,
            report_id=report_id,
            submitted_date=submitted_date,
            category=category,
            total_amount=total_amount,
            currency=currency,
            description=description,
            report_url=report_url,
            employee_image_url=employee_image_url,
        )
