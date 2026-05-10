use crate::builders::*;
use crate::enums::*;
use crate::models::Card;

/// Provides helper methods for creating Microsoft Teams-style Adaptive Cards.
pub struct TeamsCards;

/// Parameters for an approval request card.
pub struct ApprovalCardParams {
    pub requester_name: String,
    pub submitted_date: String,
    pub title: String,
    pub category: String,
    pub amount: String,
    pub business_unit: String,
    pub due_date: String,
    pub description: String,
    pub requester_image_url: Option<String>,
}

/// Parameters for a status update notification card.
pub struct StatusUpdateCardParams {
    pub card_title: String,
    pub team_name: String,
    pub update_date: String,
    pub project: String,
    pub status: String,
    pub sprint: String,
    pub completion: String,
    pub updated_by: String,
    pub notes: String,
    pub project_url: String,
}

/// Parameters for a task assignment notification card.
pub struct TaskUpdateCardParams {
    pub task_name: String,
    pub project: String,
    pub assigned_by: String,
    pub due_date: String,
    pub estimate: String,
    pub priority: String,
    pub description: String,
    pub task_url: String,
}

/// Parameters for a meeting reminder card.
pub struct MeetingReminderCardParams {
    pub meeting_title: String,
    pub organizer: String,
    pub date: String,
    pub time: String,
    pub location: String,
    pub attendees: String,
    pub agenda: String,
    pub join_url: String,
    pub details_url: String,
}

/// Parameters for an expense report card.
pub struct ExpenseReportCardParams {
    pub employee_name: String,
    pub employee_job_title: String,
    pub report_id: String,
    pub submitted_date: String,
    pub category: String,
    pub total_amount: String,
    pub currency: String,
    pub description: String,
    pub report_url: String,
    pub employee_image_url: Option<String>,
}

impl TeamsCards {
    /// Creates an approval request card with Approve and Decline actions.
    pub fn approval_card(p: &ApprovalCardParams) -> Card {
        let requester_name = p.requester_name.clone();
        let submitted_date = p.submitted_date.clone();
        let requester_image_url = p.requester_image_url.clone();
        let title = p.title.clone();
        let category = p.category.clone();
        let amount = p.amount.clone();
        let business_unit = p.business_unit.clone();
        let due_date = p.due_date.clone();
        let description = p.description.clone();

        AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_column_set(move |cs| {
                if let Some(ref img_url) = requester_image_url {
                    let url = img_url.clone();
                    cs.add_column_with_width("auto", move |col| {
                        let u = url.clone();
                        col.add_image(move |img| {
                            img.with_url(&u)
                                .with_size(ImageSize::Small)
                                .with_style(ImageStyle::Person);
                        });
                    });
                }
                let rn = requester_name.clone();
                let sd = submitted_date.clone();
                cs.add_column_with_width("stretch", move |col| {
                    let rn2 = rn.clone();
                    let sd2 = sd.clone();
                    col.with_vertical_content_alignment(VerticalAlignment::Center)
                        .add_text_block(move |tb| {
                            tb.with_text(&rn2)
                                .with_weight(TextWeight::Bolder)
                                .with_wrap(true);
                        })
                        .add_text_block(move |tb| {
                            tb.with_text(&sd2)
                                .with_is_subtle(true)
                                .with_size(TextSize::Small)
                                .with_wrap(true);
                        });
                });
            })
            .add_text_block(move |tb| {
                tb.with_text(&title)
                    .with_size(TextSize::Large)
                    .with_weight(TextWeight::Bolder)
                    .with_wrap(true);
            })
            .add_fact_set(move |fs| {
                fs.add_fact("Category", &category)
                    .add_fact("Amount", &amount)
                    .add_fact("Business Unit", &business_unit)
                    .add_fact("Due Date", &due_date);
            })
            .add_text_block(move |tb| {
                tb.with_text(&description)
                    .with_wrap(true)
                    .with_is_subtle(true);
            })
            .add_action(|a| {
                a.submit("Approve")
                    .with_style(ActionStyle::Positive);
            })
            .add_action(|a| {
                a.submit("Decline")
                    .with_style(ActionStyle::Destructive);
            })
            .build()
    }

    /// Creates a status update notification card.
    pub fn status_update_card(p: &StatusUpdateCardParams) -> Card {
        let card_title = p.card_title.clone();
        let subtitle = format!("{} \u{2022} {}", p.team_name, p.update_date);
        let project = p.project.clone();
        let status = p.status.clone();
        let sprint = p.sprint.clone();
        let completion = p.completion.clone();
        let updated_by = p.updated_by.clone();
        let notes = p.notes.clone();
        let project_url = p.project_url.clone();

        AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_container(move |c| {
                let ct = card_title.clone();
                let st = subtitle.clone();
                c.with_style(ContainerStyle::Emphasis)
                    .add_column_set(move |cs| {
                        let ct2 = ct.clone();
                        let st2 = st.clone();
                        cs.add_column_with_width("stretch", move |col| {
                            let ct3 = ct2.clone();
                            let st3 = st2.clone();
                            col.add_text_block(move |tb| {
                                tb.with_text(&ct3)
                                    .with_size(TextSize::Large)
                                    .with_weight(TextWeight::Bolder)
                                    .with_wrap(true);
                            })
                            .add_text_block(move |tb| {
                                tb.with_text(&st3)
                                    .with_is_subtle(true)
                                    .with_size(TextSize::Small)
                                    .with_wrap(true);
                            });
                        });
                    });
            })
            .add_fact_set(move |fs| {
                fs.add_fact("Project", &project)
                    .add_fact("Status", &status)
                    .add_fact("Sprint", &sprint)
                    .add_fact("Completion", &completion)
                    .add_fact("Updated By", &updated_by);
            })
            .add_text_block(move |tb| {
                tb.with_text(&notes).with_wrap(true);
            })
            .add_action(move |a| {
                a.open_url(&project_url).with_title("View Project");
            })
            .build()
    }

    /// Creates a task assignment notification card.
    pub fn task_update_card(p: &TaskUpdateCardParams) -> Card {
        let priority = p.priority.clone();
        let task_name = p.task_name.clone();
        let project = p.project.clone();
        let assigned_by = p.assigned_by.clone();
        let due_date = p.due_date.clone();
        let estimate = p.estimate.clone();
        let description = p.description.clone();
        let task_url = p.task_url.clone();

        AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_column_set(move |cs| {
                cs.add_column_with_width("stretch", |col| {
                    col.add_text_block(|tb| {
                        tb.with_text("Task Assigned to You")
                            .with_size(TextSize::Large)
                            .with_weight(TextWeight::Bolder)
                            .with_wrap(true);
                    });
                });
                let pri = priority.clone();
                cs.add_column_with_width("auto", move |col| {
                    let pri2 = pri.clone();
                    col.with_vertical_content_alignment(VerticalAlignment::Center)
                        .add_text_block(move |tb| {
                            tb.with_text(&pri2)
                                .with_color(TextColor::Attention)
                                .with_weight(TextWeight::Bolder);
                        });
                });
            })
            .add_fact_set(move |fs| {
                fs.add_fact("Task", &task_name)
                    .add_fact("Project", &project)
                    .add_fact("Assigned By", &assigned_by)
                    .add_fact("Due Date", &due_date)
                    .add_fact("Estimate", &estimate);
            })
            .add_text_block(move |tb| {
                tb.with_text(&description)
                    .with_wrap(true)
                    .with_is_subtle(true);
            })
            .add_action(move |a| {
                a.open_url(&task_url).with_title("View Task");
            })
            .add_action(|a| {
                a.submit("Acknowledge")
                    .with_style(ActionStyle::Positive);
            })
            .build()
    }

    /// Creates a meeting reminder card with join and details links.
    pub fn meeting_reminder_card(p: &MeetingReminderCardParams) -> Card {
        let meeting_title = p.meeting_title.clone();
        let organizer = p.organizer.clone();
        let date = p.date.clone();
        let time = p.time.clone();
        let location = p.location.clone();
        let attendees = p.attendees.clone();
        let agenda = p.agenda.clone();
        let join_url = p.join_url.clone();
        let details_url = p.details_url.clone();

        AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_text_block(|tb| {
                tb.with_text("\u{23f0} Meeting Starting Soon")
                    .with_size(TextSize::Large)
                    .with_weight(TextWeight::Bolder)
                    .with_wrap(true);
            })
            .add_text_block(move |tb| {
                tb.with_text(&meeting_title)
                    .with_size(TextSize::Medium)
                    .with_wrap(true);
            })
            .add_fact_set(move |fs| {
                fs.add_fact("Organizer", &organizer)
                    .add_fact("Date", &date)
                    .add_fact("Time", &time)
                    .add_fact("Location", &location)
                    .add_fact("Attendees", &attendees);
            })
            .add_text_block(move |tb| {
                tb.with_text(&agenda)
                    .with_wrap(true)
                    .with_is_subtle(true);
            })
            .add_action(move |a| {
                a.open_url(&join_url)
                    .with_title("Join Meeting")
                    .with_style(ActionStyle::Positive);
            })
            .add_action(move |a| {
                a.open_url(&details_url).with_title("View Details");
            })
            .build()
    }

    /// Creates an expense report card for finance team review.
    pub fn expense_report_card(p: &ExpenseReportCardParams) -> Card {
        let employee_name = p.employee_name.clone();
        let employee_job_title = p.employee_job_title.clone();
        let employee_image_url = p.employee_image_url.clone();
        let report_id = p.report_id.clone();
        let submitted_date = p.submitted_date.clone();
        let category = p.category.clone();
        let total_amount = p.total_amount.clone();
        let currency = p.currency.clone();
        let description = p.description.clone();
        let report_url = p.report_url.clone();

        AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_container(|c| {
                c.with_style(ContainerStyle::Emphasis)
                    .add_text_block(|tb| {
                        tb.with_text("Expense Report Submitted")
                            .with_size(TextSize::Large)
                            .with_weight(TextWeight::Bolder)
                            .with_wrap(true);
                    })
                    .add_text_block(|tb| {
                        tb.with_text("Awaiting your review and approval")
                            .with_is_subtle(true)
                            .with_wrap(true);
                    });
            })
            .add_column_set(move |cs| {
                if let Some(ref img_url) = employee_image_url {
                    let url = img_url.clone();
                    cs.add_column_with_width("auto", move |col| {
                        let u = url.clone();
                        col.add_image(move |img| {
                            img.with_url(&u)
                                .with_size(ImageSize::Small)
                                .with_style(ImageStyle::Person);
                        });
                    });
                }
                let en = employee_name.clone();
                let ejt = employee_job_title.clone();
                cs.add_column_with_width("stretch", move |col| {
                    let en2 = en.clone();
                    let ejt2 = ejt.clone();
                    col.with_vertical_content_alignment(VerticalAlignment::Center)
                        .add_text_block(move |tb| {
                            tb.with_text(&en2)
                                .with_weight(TextWeight::Bolder)
                                .with_wrap(true);
                        })
                        .add_text_block(move |tb| {
                            tb.with_text(&ejt2)
                                .with_is_subtle(true)
                                .with_size(TextSize::Small)
                                .with_wrap(true);
                        });
                });
            })
            .add_fact_set(move |fs| {
                fs.add_fact("Report ID", &report_id)
                    .add_fact("Submitted", &submitted_date)
                    .add_fact("Category", &category)
                    .add_fact("Total Amount", &total_amount)
                    .add_fact("Currency", &currency);
            })
            .add_text_block(move |tb| {
                tb.with_text(&description)
                    .with_wrap(true)
                    .with_is_subtle(true);
            })
            .add_action(|a| {
                a.submit("Approve")
                    .with_style(ActionStyle::Positive);
            })
            .add_action(|a| {
                a.submit("Reject")
                    .with_style(ActionStyle::Destructive);
            })
            .add_action(move |a| {
                a.open_url(&report_url).with_title("View Report");
            })
            .build()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn approval_card_has_actions() {
        let card = TeamsCards::approval_card(&ApprovalCardParams {
            requester_name: "John".into(),
            submitted_date: "2024-01-01".into(),
            title: "Test".into(),
            category: "Travel".into(),
            amount: "$500".into(),
            business_unit: "Engineering".into(),
            due_date: "2024-02-01".into(),
            description: "Trip".into(),
            requester_image_url: None,
        });
        assert!(card.get("actions").is_some());
        let actions = card["actions"].as_array().unwrap();
        assert_eq!(actions.len(), 2);
    }

    #[test]
    fn status_update_card_has_body() {
        let card = TeamsCards::status_update_card(&StatusUpdateCardParams {
            card_title: "Update".into(),
            team_name: "Team A".into(),
            update_date: "2024-01-01".into(),
            project: "Project X".into(),
            status: "On Track".into(),
            sprint: "Sprint 5".into(),
            completion: "75%".into(),
            updated_by: "Alice".into(),
            notes: "Good progress".into(),
            project_url: "https://example.com".into(),
        });
        assert!(card.get("body").is_some());
    }

    #[test]
    fn meeting_reminder_card_has_actions() {
        let card = TeamsCards::meeting_reminder_card(&MeetingReminderCardParams {
            meeting_title: "Standup".into(),
            organizer: "Bob".into(),
            date: "2024-01-01".into(),
            time: "09:00".into(),
            location: "Room A".into(),
            attendees: "Team".into(),
            agenda: "Weekly sync".into(),
            join_url: "https://example.com/join".into(),
            details_url: "https://example.com/details".into(),
        });
        let actions = card["actions"].as_array().unwrap();
        assert_eq!(actions.len(), 2);
    }
}
