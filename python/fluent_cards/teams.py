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
        """Creates an approval request card with Approve and Decline actions.

        Args:
            requester_name: The display name of the person making the request.
            submitted_date: The date the request was submitted.
            title: The title of the approval request.
            category: The category of the request.
            amount: The amount being requested.
            business_unit: The business unit associated with the request.
            due_date: The due date for the approval decision.
            description: A description of the approval request.
            requester_image_url: Optional URL of the requester's avatar image.

        Returns:
            The configured Adaptive Card dictionary.
        """
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
        """Creates a status update notification card showing the current state of a project or task.

        Args:
            card_title: The title displayed on the card.
            team_name: The name of the team posting the update.
            update_date: The date of the status update.
            project: The project name.
            status: The current project status.
            sprint: The current sprint identifier.
            completion: The completion percentage or label.
            updated_by: The name of the person posting the update.
            notes: Additional notes or comments about the status.
            project_url: URL linking to the project.

        Returns:
            The configured Adaptive Card dictionary.
        """
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
        """Creates a task assignment notification card informing the recipient of a newly assigned task.

        Args:
            task_name: The name of the assigned task.
            project: The project the task belongs to.
            assigned_by: The name of the person who assigned the task.
            due_date: The due date for the task.
            estimate: The time estimate for the task.
            priority: The priority label for the task.
            description: A description of the task.
            task_url: URL linking to the task details.

        Returns:
            The configured Adaptive Card dictionary.
        """
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
        """Creates a meeting reminder card with meeting details and a join link.

        Args:
            meeting_title: The title of the meeting.
            organizer: The name of the meeting organizer.
            date: The date of the meeting.
            time: The time of the meeting.
            location: The location or meeting room.
            attendees: A description of the attendees.
            agenda: The meeting agenda or summary.
            join_url: URL to join the meeting.
            details_url: URL to view full meeting details.

        Returns:
            The configured Adaptive Card dictionary.
        """
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
        """Creates an expense report card for finance team review with Approve and Reject actions.

        Args:
            employee_name: The name of the employee submitting the expense report.
            employee_job_title: The job title of the employee.
            report_id: The unique identifier for the expense report.
            submitted_date: The date the report was submitted.
            category: The expense category.
            total_amount: The total amount of the expense.
            currency: The currency of the expense.
            description: A description of the expenses.
            report_url: URL linking to the full expense report.
            employee_image_url: Optional URL of the employee's avatar image.

        Returns:
            The configured Adaptive Card dictionary.
        """
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
