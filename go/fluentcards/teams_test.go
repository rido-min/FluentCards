package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestTeamsCards_ApprovalCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.TeamsCards.ApprovalCard(fluentcards.ApprovalCardParams{
		RequesterName: "Alice",
		SubmittedDate: "2026-01-15",
		Title:         "Budget Approval",
		Category:      "Finance",
		Amount:        "$5,000",
		BusinessUnit:  "Engineering",
		DueDate:       "2026-02-01",
		Description:   "Q1 budget request",
	})
	assert.Equal(t, "AdaptiveCard", card["type"])
	assert.Equal(t, "1.5", card["version"])
	body := card["body"].([]any)
	assert.True(t, len(body) >= 3, "expected at least 3 body elements")
	actions := card["actions"].([]any)
	require.Len(t, actions, 2)
	assert.Equal(t, "Approve", actions[0].(map[string]any)["title"])
	assert.Equal(t, "Decline", actions[1].(map[string]any)["title"])
}

func TestTeamsCards_ApprovalCard_WithImage(t *testing.T) {
	t.Parallel()
	card := fluentcards.TeamsCards.ApprovalCard(fluentcards.ApprovalCardParams{
		RequesterName:    "Bob",
		SubmittedDate:    "2026-01-15",
		Title:            "PTO Request",
		Category:         "HR",
		Amount:           "5 days",
		BusinessUnit:     "Product",
		DueDate:          "2026-01-20",
		Description:      "Taking a vacation",
		RequesterImageURL: "https://example.com/bob.png",
	})
	// ColumnSet should have 2 columns (image + text)
	cs := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "ColumnSet", cs["type"])
	cols := cs["columns"].([]any)
	assert.Len(t, cols, 2)
}

func TestTeamsCards_StatusUpdateCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.TeamsCards.StatusUpdateCard(fluentcards.StatusUpdateCardParams{
		CardTitle:  "Sprint Status",
		TeamName:   "Platform Team",
		UpdateDate: "2026-01-20",
		Project:    "FluentCards",
		Status:     "On Track",
		Sprint:     "Sprint 12",
		Completion: "75%",
		UpdatedBy:  "Alice",
		Notes:      "All good",
		ProjectURL: "https://example.com/project",
	})
	assert.Equal(t, "AdaptiveCard", card["type"])
	actions := card["actions"].([]any)
	require.Len(t, actions, 1)
	assert.Equal(t, "View Project", actions[0].(map[string]any)["title"])
}

func TestTeamsCards_TaskUpdateCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.TeamsCards.TaskUpdateCard(fluentcards.TaskUpdateCardParams{
		TaskName:    "Build Go SDK",
		Project:     "FluentCards",
		AssignedBy:  "Manager",
		DueDate:     "2026-03-01",
		Estimate:    "2 weeks",
		Priority:    "High",
		Description: "Port the library to Go",
		TaskURL:     "https://example.com/task/1",
	})
	assert.Equal(t, "AdaptiveCard", card["type"])
	actions := card["actions"].([]any)
	assert.Len(t, actions, 2)
}

func TestTeamsCards_MeetingReminderCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.TeamsCards.MeetingReminderCard(fluentcards.MeetingReminderCardParams{
		MeetingTitle: "Sprint Planning",
		Organizer:    "Alice",
		Date:         "2026-01-22",
		Time:         "10:00 AM",
		Location:     "Teams",
		Attendees:    "Alice, Bob, Charlie",
		Agenda:       "Plan sprint 13",
		JoinURL:      "https://teams.microsoft.com/meeting/abc",
		DetailsURL:   "https://calendar.example.com/meeting/abc",
	})
	assert.Equal(t, "AdaptiveCard", card["type"])
	body := card["body"].([]any)
	assert.True(t, len(body) >= 3)
	actions := card["actions"].([]any)
	require.Len(t, actions, 2)
	assert.Equal(t, "Join Meeting", actions[0].(map[string]any)["title"])
}

func TestTeamsCards_ExpenseReportCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.TeamsCards.ExpenseReportCard(fluentcards.ExpenseReportCardParams{
		EmployeeName:     "Bob",
		EmployeeJobTitle: "Engineer",
		ReportID:         "EXP-001",
		SubmittedDate:    "2026-01-15",
		Category:         "Travel",
		TotalAmount:      "$1,200",
		Currency:         "USD",
		Description:      "Conference attendance",
		ReportURL:        "https://example.com/report/1",
	})
	assert.Equal(t, "AdaptiveCard", card["type"])
	actions := card["actions"].([]any)
	require.Len(t, actions, 3)
	assert.Equal(t, "Approve", actions[0].(map[string]any)["title"])
	assert.Equal(t, "Reject", actions[1].(map[string]any)["title"])
	assert.Equal(t, "View Report", actions[2].(map[string]any)["title"])
}

func TestTeamsCards_AllCardsPassValidation(t *testing.T) {
	t.Parallel()
	cards := []fluentcards.Card{
		fluentcards.TeamsCards.ApprovalCard(fluentcards.ApprovalCardParams{
			RequesterName: "Alice", SubmittedDate: "2026-01-15", Title: "T",
			Category: "C", Amount: "A", BusinessUnit: "B", DueDate: "D", Description: "D",
		}),
		fluentcards.TeamsCards.StatusUpdateCard(fluentcards.StatusUpdateCardParams{
			CardTitle: "S", TeamName: "T", UpdateDate: "U", Project: "P",
			Status: "S", Sprint: "S", Completion: "C", UpdatedBy: "U",
			Notes: "N", ProjectURL: "https://example.com",
		}),
		fluentcards.TeamsCards.TaskUpdateCard(fluentcards.TaskUpdateCardParams{
			TaskName: "T", Project: "P", AssignedBy: "A", DueDate: "D",
			Estimate: "E", Priority: "H", Description: "D", TaskURL: "https://example.com",
		}),
		fluentcards.TeamsCards.MeetingReminderCard(fluentcards.MeetingReminderCardParams{
			MeetingTitle: "M", Organizer: "O", Date: "D", Time: "T",
			Location: "L", Attendees: "A", Agenda: "A",
			JoinURL: "https://example.com", DetailsURL: "https://example.com",
		}),
		fluentcards.TeamsCards.ExpenseReportCard(fluentcards.ExpenseReportCardParams{
			EmployeeName: "E", EmployeeJobTitle: "J", ReportID: "R", SubmittedDate: "S",
			Category: "C", TotalAmount: "T", Currency: "USD", Description: "D",
			ReportURL: "https://example.com",
		}),
	}
	for _, card := range cards {
		issues := fluentcards.Validate(card)
		for _, i := range issues {
			assert.NotEqual(t, fluentcards.ValidationSeverityError, i.Severity,
				"Teams card has error: [%s] %s: %s", i.Code, i.Path, i.Message)
		}
	}
}
