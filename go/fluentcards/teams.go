package fluentcards

// TeamsCards provides helper methods for creating Microsoft Teams-style Adaptive Cards.
// These reflect common patterns from the Teams Adaptive Card Samples collection.
var TeamsCards = teamsCards{}

type teamsCards struct{}

// ApprovalCardParams holds the parameters for an approval request card.
type ApprovalCardParams struct {
	RequesterName    string
	SubmittedDate    string
	Title            string
	Category         string
	Amount           string
	BusinessUnit     string
	DueDate          string
	Description      string
	RequesterImageURL string // optional
}

// ApprovalCard creates an approval request card with Approve and Decline actions.
func (teamsCards) ApprovalCard(p ApprovalCardParams) Card {
	b := NewAdaptiveCardBuilder().WithVersion("1.5")

	b.AddColumnSet(func(cs *ColumnSetBuilder) {
		if p.RequesterImageURL != "" {
			cs.AddColumnWithWidth("auto", func(col *ColumnBuilder) {
				col.AddImage(func(img *ImageBuilder) {
					img.WithURL(p.RequesterImageURL).
						WithSize(ImageSizeSmall).
						WithStyle(ImageStylePerson)
				})
			})
		}
		cs.AddColumnWithWidth("stretch", func(col *ColumnBuilder) {
			col.WithVerticalContentAlignment(VerticalAlignmentCenter).
				AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText(p.RequesterName).WithWeight(TextWeightBolder).WithWrap(true)
				}).
				AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText(p.SubmittedDate).WithIsSubtle(true).WithSize(TextSizeSmall).WithWrap(true)
				})
		})
	})

	return b.
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.Title).WithSize(TextSizeLarge).WithWeight(TextWeightBolder).WithWrap(true)
		}).
		AddFactSet(func(fs *FactSetBuilder) {
			fs.AddFact("Category", p.Category).
				AddFact("Amount", p.Amount).
				AddFact("Business Unit", p.BusinessUnit).
				AddFact("Due Date", p.DueDate)
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.Description).WithWrap(true).WithIsSubtle(true)
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("Approve").WithStyle(ActionStylePositive)
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("Decline").WithStyle(ActionStyleDestructive)
		}).
		Build()
}

// StatusUpdateCardParams holds the parameters for a status update notification card.
type StatusUpdateCardParams struct {
	CardTitle  string
	TeamName   string
	UpdateDate string
	Project    string
	Status     string
	Sprint     string
	Completion string
	UpdatedBy  string
	Notes      string
	ProjectURL string
}

// StatusUpdateCard creates a status update notification card.
func (teamsCards) StatusUpdateCard(p StatusUpdateCardParams) Card {
	return NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddContainer(func(c *ContainerBuilder) {
			c.WithStyle(ContainerStyleEmphasis).
				AddColumnSet(func(cs *ColumnSetBuilder) {
					cs.AddColumnWithWidth("stretch", func(col *ColumnBuilder) {
						col.AddTextBlock(func(tb *TextBlockBuilder) {
							tb.WithText(p.CardTitle).WithSize(TextSizeLarge).WithWeight(TextWeightBolder).WithWrap(true)
						}).AddTextBlock(func(tb *TextBlockBuilder) {
							tb.WithText(p.TeamName + " \u2022 " + p.UpdateDate).
								WithIsSubtle(true).WithSize(TextSizeSmall).WithWrap(true)
						})
					})
				})
		}).
		AddFactSet(func(fs *FactSetBuilder) {
			fs.AddFact("Project", p.Project).
				AddFact("Status", p.Status).
				AddFact("Sprint", p.Sprint).
				AddFact("Completion", p.Completion).
				AddFact("Updated By", p.UpdatedBy)
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.Notes).WithWrap(true)
		}).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL(p.ProjectURL).WithTitle("View Project")
		}).
		Build()
}

// TaskUpdateCardParams holds the parameters for a task assignment notification card.
type TaskUpdateCardParams struct {
	TaskName   string
	Project    string
	AssignedBy string
	DueDate    string
	Estimate   string
	Priority   string
	Description string
	TaskURL    string
}

// TaskUpdateCard creates a task assignment notification card.
func (teamsCards) TaskUpdateCard(p TaskUpdateCardParams) Card {
	return NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddColumnSet(func(cs *ColumnSetBuilder) {
			cs.AddColumnWithWidth("stretch", func(col *ColumnBuilder) {
				col.AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText("Task Assigned to You").WithSize(TextSizeLarge).WithWeight(TextWeightBolder).WithWrap(true)
				})
			}).AddColumnWithWidth("auto", func(col *ColumnBuilder) {
				col.WithVerticalContentAlignment(VerticalAlignmentCenter).
					AddTextBlock(func(tb *TextBlockBuilder) {
						tb.WithText(p.Priority).WithColor(TextColorAttention).WithWeight(TextWeightBolder)
					})
			})
		}).
		AddFactSet(func(fs *FactSetBuilder) {
			fs.AddFact("Task", p.TaskName).
				AddFact("Project", p.Project).
				AddFact("Assigned By", p.AssignedBy).
				AddFact("Due Date", p.DueDate).
				AddFact("Estimate", p.Estimate)
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.Description).WithWrap(true).WithIsSubtle(true)
		}).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL(p.TaskURL).WithTitle("View Task")
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("Acknowledge").WithStyle(ActionStylePositive)
		}).
		Build()
}

// MeetingReminderCardParams holds the parameters for a meeting reminder card.
type MeetingReminderCardParams struct {
	MeetingTitle string
	Organizer    string
	Date         string
	Time         string
	Location     string
	Attendees    string
	Agenda       string
	JoinURL      string
	DetailsURL   string
}

// MeetingReminderCard creates a meeting reminder card with join and details links.
func (teamsCards) MeetingReminderCard(p MeetingReminderCardParams) Card {
	return NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("\u23f0 Meeting Starting Soon").WithSize(TextSizeLarge).WithWeight(TextWeightBolder).WithWrap(true)
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.MeetingTitle).WithSize(TextSizeMedium).WithWrap(true)
		}).
		AddFactSet(func(fs *FactSetBuilder) {
			fs.AddFact("Organizer", p.Organizer).
				AddFact("Date", p.Date).
				AddFact("Time", p.Time).
				AddFact("Location", p.Location).
				AddFact("Attendees", p.Attendees)
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.Agenda).WithWrap(true).WithIsSubtle(true)
		}).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL(p.JoinURL).WithTitle("Join Meeting").WithStyle(ActionStylePositive)
		}).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL(p.DetailsURL).WithTitle("View Details")
		}).
		Build()
}

// ExpenseReportCardParams holds the parameters for an expense report card.
type ExpenseReportCardParams struct {
	EmployeeName     string
	EmployeeJobTitle string
	ReportID         string
	SubmittedDate    string
	Category         string
	TotalAmount      string
	Currency         string
	Description      string
	ReportURL        string
	EmployeeImageURL string // optional
}

// ExpenseReportCard creates an expense report card for finance team review.
func (teamsCards) ExpenseReportCard(p ExpenseReportCardParams) Card {
	b := NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddContainer(func(c *ContainerBuilder) {
			c.WithStyle(ContainerStyleEmphasis).
				AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText("Expense Report Submitted").WithSize(TextSizeLarge).WithWeight(TextWeightBolder).WithWrap(true)
				}).
				AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText("Awaiting your review and approval").WithIsSubtle(true).WithWrap(true)
				})
		})

	b.AddColumnSet(func(cs *ColumnSetBuilder) {
		if p.EmployeeImageURL != "" {
			cs.AddColumnWithWidth("auto", func(col *ColumnBuilder) {
				col.AddImage(func(img *ImageBuilder) {
					img.WithURL(p.EmployeeImageURL).WithSize(ImageSizeSmall).WithStyle(ImageStylePerson)
				})
			})
		}
		cs.AddColumnWithWidth("stretch", func(col *ColumnBuilder) {
			col.WithVerticalContentAlignment(VerticalAlignmentCenter).
				AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText(p.EmployeeName).WithWeight(TextWeightBolder).WithWrap(true)
				}).
				AddTextBlock(func(tb *TextBlockBuilder) {
					tb.WithText(p.EmployeeJobTitle).WithIsSubtle(true).WithSize(TextSizeSmall).WithWrap(true)
				})
		})
	})

	return b.
		AddFactSet(func(fs *FactSetBuilder) {
			fs.AddFact("Report ID", p.ReportID).
				AddFact("Submitted", p.SubmittedDate).
				AddFact("Category", p.Category).
				AddFact("Total Amount", p.TotalAmount).
				AddFact("Currency", p.Currency)
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(p.Description).WithWrap(true).WithIsSubtle(true)
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("Approve").WithStyle(ActionStylePositive)
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("Reject").WithStyle(ActionStyleDestructive)
		}).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL(p.ReportURL).WithTitle("View Report")
		}).
		Build()
}
