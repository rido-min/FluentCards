package io.fluentcards;

import java.util.Map;

/**
 * Utility class with static methods for creating Microsoft Teams-style Adaptive Cards.
 * These reflect common patterns from the Teams Adaptive Card Samples collection.
 */
public final class TeamsCards {
    private TeamsCards() {}

    // ---- Approval Card ----

    /**
     * Parameters for an approval request card.
     */
    public static class ApprovalCardParams {
        public final String requesterName;
        public final String submittedDate;
        public final String title;
        public final String category;
        public final String amount;
        public final String businessUnit;
        public final String dueDate;
        public final String description;
        public final String requesterImageUrl;

        public ApprovalCardParams(String requesterName, String submittedDate,
                                  String title, String category, String amount,
                                  String businessUnit, String dueDate,
                                  String description, String requesterImageUrl) {
            this.requesterName = requesterName;
            this.submittedDate = submittedDate;
            this.title = title;
            this.category = category;
            this.amount = amount;
            this.businessUnit = businessUnit;
            this.dueDate = dueDate;
            this.description = description;
            this.requesterImageUrl = requesterImageUrl;
        }
    }

    /**
     * Creates an approval request card with Approve and Decline actions.
     */
    public static Map<String, Object> approvalCard(ApprovalCardParams p) {
        AdaptiveCardBuilder b = AdaptiveCardBuilder.create().withVersion("1.5");

        b.addColumnSet(cs -> {
            if (p.requesterImageUrl != null && !p.requesterImageUrl.isEmpty()) {
                cs.addColumnWithWidth("auto", col ->
                    col.addImage(img ->
                        img.withUrl(p.requesterImageUrl)
                           .withSize(ImageSize.SMALL)
                           .withStyle(ImageStyle.PERSON)));
            }
            cs.addColumnWithWidth("stretch", col ->
                col.withVerticalContentAlignment(VerticalAlignment.CENTER)
                   .addTextBlock(tb ->
                       tb.withText(p.requesterName)
                         .withWeight(TextWeight.BOLDER)
                         .withWrap(true))
                   .addTextBlock(tb ->
                       tb.withText(p.submittedDate)
                         .withIsSubtle(true)
                         .withSize(TextSize.SMALL)
                         .withWrap(true)));
        });

        return b
            .addTextBlock(tb ->
                tb.withText(p.title)
                  .withSize(TextSize.LARGE)
                  .withWeight(TextWeight.BOLDER)
                  .withWrap(true))
            .addFactSet(fs ->
                fs.addFact("Category", p.category)
                  .addFact("Amount", p.amount)
                  .addFact("Business Unit", p.businessUnit)
                  .addFact("Due Date", p.dueDate))
            .addTextBlock(tb ->
                tb.withText(p.description)
                  .withWrap(true)
                  .withIsSubtle(true))
            .addAction(a ->
                a.submit("Approve").withStyle(ActionStyle.POSITIVE))
            .addAction(a ->
                a.submit("Decline").withStyle(ActionStyle.DESTRUCTIVE))
            .build();
    }

    // ---- Status Update Card ----

    /**
     * Parameters for a status update notification card.
     */
    public static class StatusUpdateCardParams {
        public final String cardTitle;
        public final String teamName;
        public final String updateDate;
        public final String project;
        public final String status;
        public final String sprint;
        public final String completion;
        public final String updatedBy;
        public final String notes;
        public final String projectUrl;

        public StatusUpdateCardParams(String cardTitle, String teamName,
                                      String updateDate, String project,
                                      String status, String sprint,
                                      String completion, String updatedBy,
                                      String notes, String projectUrl) {
            this.cardTitle = cardTitle;
            this.teamName = teamName;
            this.updateDate = updateDate;
            this.project = project;
            this.status = status;
            this.sprint = sprint;
            this.completion = completion;
            this.updatedBy = updatedBy;
            this.notes = notes;
            this.projectUrl = projectUrl;
        }
    }

    /**
     * Creates a status update notification card.
     */
    public static Map<String, Object> statusUpdateCard(StatusUpdateCardParams p) {
        return AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addContainer(c ->
                c.withStyle(ContainerStyle.EMPHASIS)
                 .addColumnSet(cs ->
                     cs.addColumnWithWidth("stretch", col ->
                         col.addTextBlock(tb ->
                                 tb.withText(p.cardTitle)
                                   .withSize(TextSize.LARGE)
                                   .withWeight(TextWeight.BOLDER)
                                   .withWrap(true))
                            .addTextBlock(tb ->
                                 tb.withText(p.teamName + " \u2022 " + p.updateDate)
                                   .withIsSubtle(true)
                                   .withSize(TextSize.SMALL)
                                   .withWrap(true)))))
            .addFactSet(fs ->
                fs.addFact("Project", p.project)
                  .addFact("Status", p.status)
                  .addFact("Sprint", p.sprint)
                  .addFact("Completion", p.completion)
                  .addFact("Updated By", p.updatedBy))
            .addTextBlock(tb ->
                tb.withText(p.notes).withWrap(true))
            .addAction(a ->
                a.openUrl(p.projectUrl).withTitle("View Project"))
            .build();
    }

    // ---- Task Update Card ----

    /**
     * Parameters for a task assignment notification card.
     */
    public static class TaskUpdateCardParams {
        public final String taskName;
        public final String project;
        public final String assignedBy;
        public final String dueDate;
        public final String estimate;
        public final String priority;
        public final String description;
        public final String taskUrl;

        public TaskUpdateCardParams(String taskName, String project,
                                    String assignedBy, String dueDate,
                                    String estimate, String priority,
                                    String description, String taskUrl) {
            this.taskName = taskName;
            this.project = project;
            this.assignedBy = assignedBy;
            this.dueDate = dueDate;
            this.estimate = estimate;
            this.priority = priority;
            this.description = description;
            this.taskUrl = taskUrl;
        }
    }

    /**
     * Creates a task assignment notification card.
     */
    public static Map<String, Object> taskUpdateCard(TaskUpdateCardParams p) {
        return AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addColumnSet(cs ->
                cs.addColumnWithWidth("stretch", col ->
                    col.addTextBlock(tb ->
                        tb.withText("Task Assigned to You")
                          .withSize(TextSize.LARGE)
                          .withWeight(TextWeight.BOLDER)
                          .withWrap(true)))
                  .addColumnWithWidth("auto", col ->
                    col.withVerticalContentAlignment(VerticalAlignment.CENTER)
                       .addTextBlock(tb ->
                           tb.withText(p.priority)
                             .withColor(TextColor.ATTENTION)
                             .withWeight(TextWeight.BOLDER))))
            .addFactSet(fs ->
                fs.addFact("Task", p.taskName)
                  .addFact("Project", p.project)
                  .addFact("Assigned By", p.assignedBy)
                  .addFact("Due Date", p.dueDate)
                  .addFact("Estimate", p.estimate))
            .addTextBlock(tb ->
                tb.withText(p.description)
                  .withWrap(true)
                  .withIsSubtle(true))
            .addAction(a ->
                a.openUrl(p.taskUrl).withTitle("View Task"))
            .addAction(a ->
                a.submit("Acknowledge").withStyle(ActionStyle.POSITIVE))
            .build();
    }

    // ---- Meeting Reminder Card ----

    /**
     * Parameters for a meeting reminder card.
     */
    public static class MeetingReminderCardParams {
        public final String meetingTitle;
        public final String organizer;
        public final String date;
        public final String time;
        public final String location;
        public final String attendees;
        public final String agenda;
        public final String joinUrl;
        public final String detailsUrl;

        public MeetingReminderCardParams(String meetingTitle, String organizer,
                                         String date, String time,
                                         String location, String attendees,
                                         String agenda, String joinUrl,
                                         String detailsUrl) {
            this.meetingTitle = meetingTitle;
            this.organizer = organizer;
            this.date = date;
            this.time = time;
            this.location = location;
            this.attendees = attendees;
            this.agenda = agenda;
            this.joinUrl = joinUrl;
            this.detailsUrl = detailsUrl;
        }
    }

    /**
     * Creates a meeting reminder card with join and details links.
     */
    public static Map<String, Object> meetingReminderCard(MeetingReminderCardParams p) {
        return AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addTextBlock(tb ->
                tb.withText("\u23f0 Meeting Starting Soon")
                  .withSize(TextSize.LARGE)
                  .withWeight(TextWeight.BOLDER)
                  .withWrap(true))
            .addTextBlock(tb ->
                tb.withText(p.meetingTitle)
                  .withSize(TextSize.MEDIUM)
                  .withWrap(true))
            .addFactSet(fs ->
                fs.addFact("Organizer", p.organizer)
                  .addFact("Date", p.date)
                  .addFact("Time", p.time)
                  .addFact("Location", p.location)
                  .addFact("Attendees", p.attendees))
            .addTextBlock(tb ->
                tb.withText(p.agenda)
                  .withWrap(true)
                  .withIsSubtle(true))
            .addAction(a ->
                a.openUrl(p.joinUrl)
                 .withTitle("Join Meeting")
                 .withStyle(ActionStyle.POSITIVE))
            .addAction(a ->
                a.openUrl(p.detailsUrl)
                 .withTitle("View Details"))
            .build();
    }

    // ---- Expense Report Card ----

    /**
     * Parameters for an expense report card.
     */
    public static class ExpenseReportCardParams {
        public final String employeeName;
        public final String employeeJobTitle;
        public final String reportId;
        public final String submittedDate;
        public final String category;
        public final String totalAmount;
        public final String currency;
        public final String description;
        public final String reportUrl;
        public final String employeeImageUrl;

        public ExpenseReportCardParams(String employeeName, String employeeJobTitle,
                                       String reportId, String submittedDate,
                                       String category, String totalAmount,
                                       String currency, String description,
                                       String reportUrl, String employeeImageUrl) {
            this.employeeName = employeeName;
            this.employeeJobTitle = employeeJobTitle;
            this.reportId = reportId;
            this.submittedDate = submittedDate;
            this.category = category;
            this.totalAmount = totalAmount;
            this.currency = currency;
            this.description = description;
            this.reportUrl = reportUrl;
            this.employeeImageUrl = employeeImageUrl;
        }
    }

    /**
     * Creates an expense report card for finance team review.
     */
    public static Map<String, Object> expenseReportCard(ExpenseReportCardParams p) {
        AdaptiveCardBuilder b = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addContainer(c ->
                c.withStyle(ContainerStyle.EMPHASIS)
                 .addTextBlock(tb ->
                     tb.withText("Expense Report Submitted")
                       .withSize(TextSize.LARGE)
                       .withWeight(TextWeight.BOLDER)
                       .withWrap(true))
                 .addTextBlock(tb ->
                     tb.withText("Awaiting your review and approval")
                       .withIsSubtle(true)
                       .withWrap(true)));

        b.addColumnSet(cs -> {
            if (p.employeeImageUrl != null && !p.employeeImageUrl.isEmpty()) {
                cs.addColumnWithWidth("auto", col ->
                    col.addImage(img ->
                        img.withUrl(p.employeeImageUrl)
                           .withSize(ImageSize.SMALL)
                           .withStyle(ImageStyle.PERSON)));
            }
            cs.addColumnWithWidth("stretch", col ->
                col.withVerticalContentAlignment(VerticalAlignment.CENTER)
                   .addTextBlock(tb ->
                       tb.withText(p.employeeName)
                         .withWeight(TextWeight.BOLDER)
                         .withWrap(true))
                   .addTextBlock(tb ->
                       tb.withText(p.employeeJobTitle)
                         .withIsSubtle(true)
                         .withSize(TextSize.SMALL)
                         .withWrap(true)));
        });

        return b
            .addFactSet(fs ->
                fs.addFact("Report ID", p.reportId)
                  .addFact("Submitted", p.submittedDate)
                  .addFact("Category", p.category)
                  .addFact("Total Amount", p.totalAmount)
                  .addFact("Currency", p.currency))
            .addTextBlock(tb ->
                tb.withText(p.description)
                  .withWrap(true)
                  .withIsSubtle(true))
            .addAction(a ->
                a.submit("Approve").withStyle(ActionStyle.POSITIVE))
            .addAction(a ->
                a.submit("Reject").withStyle(ActionStyle.DESTRUCTIVE))
            .addAction(a ->
                a.openUrl(p.reportUrl).withTitle("View Report"))
            .build();
    }
}
