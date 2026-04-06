import type { AdaptiveCard } from './models.js';
import { ActionStyle, ContainerStyle, ImageSize, ImageStyle, TextColor, TextSize, TextWeight, VerticalAlignment } from './enums.js';
import { AdaptiveCardBuilder } from './builders/AdaptiveCardBuilder.js';
import { ColumnSetBuilder } from './builders/ColumnSetBuilder.js';

// ─── Input types ─────────────────────────────────────────────────────────────

/** Input parameters for {@link TeamsAdaptiveCards.createApprovalCard}. */
export interface ApprovalCardInput {
  requesterName: string;
  submittedDate: string;
  title: string;
  category: string;
  amount: string;
  businessUnit: string;
  dueDate: string;
  description: string;
  requesterImageUrl?: string;
}

/** Input parameters for {@link TeamsAdaptiveCards.createStatusUpdateCard}. */
export interface StatusUpdateCardInput {
  cardTitle: string;
  teamName: string;
  updateDate: string;
  project: string;
  status: string;
  sprint: string;
  completion: string;
  updatedBy: string;
  notes: string;
  projectUrl: string;
}

/** Input parameters for {@link TeamsAdaptiveCards.createTaskUpdateCard}. */
export interface TaskUpdateCardInput {
  taskName: string;
  project: string;
  assignedBy: string;
  dueDate: string;
  estimate: string;
  priority: string;
  description: string;
  taskUrl: string;
}

/** Input parameters for {@link TeamsAdaptiveCards.createMeetingReminderCard}. */
export interface MeetingReminderCardInput {
  meetingTitle: string;
  organizer: string;
  date: string;
  time: string;
  location: string;
  attendees: string;
  agenda: string;
  joinUrl: string;
  detailsUrl: string;
}

/** Input parameters for {@link TeamsAdaptiveCards.createExpenseReportCard}. */
export interface ExpenseReportCardInput {
  employeeName: string;
  employeeJobTitle: string;
  reportId: string;
  submittedDate: string;
  category: string;
  totalAmount: string;
  currency: string;
  description: string;
  reportUrl: string;
  employeeImageUrl?: string;
}

// ─── Factory methods ─────────────────────────────────────────────────────────

/**
 * Provides helper methods for creating Microsoft Teams-style Adaptive Cards,
 * reflecting common patterns from the Teams Adaptive Card Samples collection.
 */
export const TeamsAdaptiveCards = {
  /**
   * Creates an approval request card with Approve and Decline actions.
   * Reflects the Teams approval sample pattern where a requester asks for sign-off on an item.
   */
  createApprovalCard(input: ApprovalCardInput): AdaptiveCard {
    const b = AdaptiveCardBuilder.create().withVersion('1.5');
    b.addColumnSet(cs => {
      if (input.requesterImageUrl) {
        cs.addColumn('auto', col => col
          .addImage(img => img
            .withUrl(input.requesterImageUrl!)
            .withSize(ImageSize.Small)
            .withStyle(ImageStyle.Person)));
      }
      cs.addColumn('stretch', col => col
        .withVerticalContentAlignment(VerticalAlignment.Center)
        .addTextBlock(tb => tb
          .withText(input.requesterName)
          .withWeight(TextWeight.Bolder)
          .withWrap(true))
        .addTextBlock(tb => tb
          .withText(input.submittedDate)
          .withIsSubtle()
          .withSize(TextSize.Small)
          .withWrap(true)));
    });
    return b
      .addTextBlock(tb => tb
        .withText(input.title)
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder)
        .withWrap(true))
      .addFactSet(fs => fs
        .addFact('Category', input.category)
        .addFact('Amount', input.amount)
        .addFact('Business Unit', input.businessUnit)
        .addFact('Due Date', input.dueDate))
      .addTextBlock(tb => tb
        .withText(input.description)
        .withWrap(true)
        .withIsSubtle())
      .addAction(a => a
        .submit('Approve')
        .withStyle(ActionStyle.Positive))
      .addAction(a => a
        .submit('Decline')
        .withStyle(ActionStyle.Destructive))
      .build();
  },

  /**
   * Creates a status update notification card showing the current state of a project or task.
   * Reflects the Teams status-update sample pattern used in project tracking scenarios.
   */
  createStatusUpdateCard(input: StatusUpdateCardInput): AdaptiveCard {
    return AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addContainer(c => c
        .withStyle(ContainerStyle.Emphasis)
        .addColumnSet(cs => cs
          .addColumn('stretch', col => col
            .addTextBlock(tb => tb
              .withText(input.cardTitle)
              .withSize(TextSize.Large)
              .withWeight(TextWeight.Bolder)
              .withWrap(true))
            .addTextBlock(tb => tb
              .withText(`${input.teamName} • ${input.updateDate}`)
              .withIsSubtle()
              .withSize(TextSize.Small)
              .withWrap(true)))))
      .addFactSet(fs => fs
        .addFact('Project', input.project)
        .addFact('Status', input.status)
        .addFact('Sprint', input.sprint)
        .addFact('Completion', input.completion)
        .addFact('Updated By', input.updatedBy))
      .addTextBlock(tb => tb
        .withText(input.notes)
        .withWrap(true))
      .addAction(a => a
        .openUrl(input.projectUrl)
        .withTitle('View Project'))
      .build();
  },

  /**
   * Creates a task assignment notification card informing the recipient of a newly assigned task.
   * Reflects the Teams task-update sample pattern used in work tracking integrations.
   */
  createTaskUpdateCard(input: TaskUpdateCardInput): AdaptiveCard {
    return AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addColumnSet(cs => cs
        .addColumn('stretch', col => col
          .addTextBlock(tb => tb
            .withText('Task Assigned to You')
            .withSize(TextSize.Large)
            .withWeight(TextWeight.Bolder)
            .withWrap(true)))
        .addColumn('auto', col => col
          .withVerticalContentAlignment(VerticalAlignment.Center)
          .addTextBlock(tb => tb
            .withText(input.priority)
            .withColor(TextColor.Attention)
            .withWeight(TextWeight.Bolder))))
      .addFactSet(fs => fs
        .addFact('Task', input.taskName)
        .addFact('Project', input.project)
        .addFact('Assigned By', input.assignedBy)
        .addFact('Due Date', input.dueDate)
        .addFact('Estimate', input.estimate))
      .addTextBlock(tb => tb
        .withText(input.description)
        .withWrap(true)
        .withIsSubtle())
      .addAction(a => a
        .openUrl(input.taskUrl)
        .withTitle('View Task'))
      .addAction(a => a
        .submit('Acknowledge')
        .withStyle(ActionStyle.Positive))
      .build();
  },

  /**
   * Creates a meeting reminder card with meeting details and a join link.
   * Reflects the Teams meeting-invite sample pattern used in calendar integration scenarios.
   */
  createMeetingReminderCard(input: MeetingReminderCardInput): AdaptiveCard {
    return AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock(tb => tb
        .withText('⏰ Meeting Starting Soon')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder)
        .withWrap(true))
      .addTextBlock(tb => tb
        .withText(input.meetingTitle)
        .withSize(TextSize.Medium)
        .withWrap(true))
      .addFactSet(fs => fs
        .addFact('Organizer', input.organizer)
        .addFact('Date', input.date)
        .addFact('Time', input.time)
        .addFact('Location', input.location)
        .addFact('Attendees', input.attendees))
      .addTextBlock(tb => tb
        .withText(input.agenda)
        .withWrap(true)
        .withIsSubtle())
      .addAction(a => a
        .openUrl(input.joinUrl)
        .withTitle('Join Meeting')
        .withStyle(ActionStyle.Positive))
      .addAction(a => a
        .openUrl(input.detailsUrl)
        .withTitle('View Details'))
      .build();
  },

  /**
   * Creates an expense report card for finance team review with Approve and Reject actions.
   * Reflects the Teams expense-report sample pattern used in finance approval workflows.
   */
  createExpenseReportCard(input: ExpenseReportCardInput): AdaptiveCard {
    const b = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addContainer(c => c
        .withStyle(ContainerStyle.Emphasis)
        .addTextBlock(tb => tb
          .withText('Expense Report Submitted')
          .withSize(TextSize.Large)
          .withWeight(TextWeight.Bolder)
          .withWrap(true))
        .addTextBlock(tb => tb
          .withText('Awaiting your review and approval')
          .withIsSubtle()
          .withWrap(true)));
    b.addColumnSet(cs => {
      if (input.employeeImageUrl) {
        cs.addColumn('auto', col => col
          .addImage(img => img
            .withUrl(input.employeeImageUrl!)
            .withSize(ImageSize.Small)
            .withStyle(ImageStyle.Person)));
      }
      cs.addColumn('stretch', col => col
        .withVerticalContentAlignment(VerticalAlignment.Center)
        .addTextBlock(tb => tb
          .withText(input.employeeName)
          .withWeight(TextWeight.Bolder)
          .withWrap(true))
        .addTextBlock(tb => tb
          .withText(input.employeeJobTitle)
          .withIsSubtle()
          .withSize(TextSize.Small)
          .withWrap(true)));
    });
    return b
      .addFactSet(fs => fs
        .addFact('Report ID', input.reportId)
        .addFact('Submitted', input.submittedDate)
        .addFact('Category', input.category)
        .addFact('Total Amount', input.totalAmount)
        .addFact('Currency', input.currency))
      .addTextBlock(tb => tb
        .withText(input.description)
        .withWrap(true)
        .withIsSubtle())
      .addAction(a => a
        .submit('Approve')
        .withStyle(ActionStyle.Positive))
      .addAction(a => a
        .submit('Reject')
        .withStyle(ActionStyle.Destructive))
      .addAction(a => a
        .openUrl(input.reportUrl)
        .withTitle('View Report'))
      .build();
  },
};
