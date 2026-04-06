package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class TeamsTest {

    @SuppressWarnings("unchecked")
    @Test
    void testApprovalCard() {
        Map<String, Object> card = TeamsCards.approvalCard(new TeamsCards.ApprovalCardParams(
                "Alice", "2026-01-15", "Budget Approval", "Finance",
                "$5,000", "Engineering", "2026-02-01", "Q1 budget request", null));
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
        List<Object> body = (List<Object>) card.get("body");
        assertNotNull(body);
        assertTrue(body.size() >= 3, "expected at least 3 body elements");
        List<Object> actions = (List<Object>) card.get("actions");
        assertEquals(2, actions.size());
        assertEquals("Approve", ((Map<String, Object>) actions.get(0)).get("title"));
        assertEquals("Decline", ((Map<String, Object>) actions.get(1)).get("title"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testStatusUpdateCard() {
        Map<String, Object> card = TeamsCards.statusUpdateCard(new TeamsCards.StatusUpdateCardParams(
                "Sprint Status", "Platform Team", "2026-01-20", "FluentCards",
                "On Track", "Sprint 12", "75%", "Alice", "All good",
                "https://example.com/project"));
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
        List<Object> body = (List<Object>) card.get("body");
        assertNotNull(body);
        assertFalse(body.isEmpty());
        List<Object> actions = (List<Object>) card.get("actions");
        assertEquals(1, actions.size());
        assertEquals("View Project", ((Map<String, Object>) actions.get(0)).get("title"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testTaskUpdateCard() {
        Map<String, Object> card = TeamsCards.taskUpdateCard(new TeamsCards.TaskUpdateCardParams(
                "Build Go SDK", "FluentCards", "Manager", "2026-03-01",
                "2 weeks", "High", "Port the library to Go",
                "https://example.com/task/1"));
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
        List<Object> body = (List<Object>) card.get("body");
        assertNotNull(body);
        assertFalse(body.isEmpty());
        List<Object> actions = (List<Object>) card.get("actions");
        assertEquals(2, actions.size());
    }

    @SuppressWarnings("unchecked")
    @Test
    void testMeetingReminderCard() {
        Map<String, Object> card = TeamsCards.meetingReminderCard(new TeamsCards.MeetingReminderCardParams(
                "Sprint Planning", "Alice", "2026-01-22", "10:00 AM",
                "Teams", "Alice, Bob, Charlie", "Plan sprint 13",
                "https://teams.microsoft.com/meeting/abc",
                "https://calendar.example.com/meeting/abc"));
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
        List<Object> body = (List<Object>) card.get("body");
        assertNotNull(body);
        assertTrue(body.size() >= 3);
        List<Object> actions = (List<Object>) card.get("actions");
        assertEquals(2, actions.size());
        assertEquals("Join Meeting", ((Map<String, Object>) actions.get(0)).get("title"));
        assertEquals("View Details", ((Map<String, Object>) actions.get(1)).get("title"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testExpenseReportCard() {
        Map<String, Object> card = TeamsCards.expenseReportCard(new TeamsCards.ExpenseReportCardParams(
                "Bob", "Engineer", "EXP-001", "2026-01-15",
                "Travel", "$1,200", "USD", "Conference attendance",
                "https://example.com/report/1", null));
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
        List<Object> body = (List<Object>) card.get("body");
        assertNotNull(body);
        assertFalse(body.isEmpty());
        List<Object> actions = (List<Object>) card.get("actions");
        assertEquals(3, actions.size());
        assertEquals("Approve", ((Map<String, Object>) actions.get(0)).get("title"));
        assertEquals("Reject", ((Map<String, Object>) actions.get(1)).get("title"));
        assertEquals("View Report", ((Map<String, Object>) actions.get(2)).get("title"));
    }
}
