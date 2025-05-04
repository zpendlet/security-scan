Security Scan - Term Project

This is a mock AWS security scanner web application built using ASP.NET Core MVC. It allows users to register, log in, and generate security scan reports based on simulated data.
Features

    User authentication
    Users can register, log in, and log out securely using ASP.NET Identity.

    Create and view reports
    Logged-in users can create a scan report. Each scan is generated from a mock JSON file and includes:

        Severity level (Low or High)

        A summary of potential issues

        A user-entered note

    Edit reports
    Users can edit the note associated with a report from the details page.

    Delete reports
    Users can delete their own reports. This works both through the standard form and via AJAX.

    AJAX functionality

        Delete operations can be performed with a confirmation popup, and the page refreshes automatically after.

        The scan summary can be toggled (shown or hidden) using JavaScript.

    Web API endpoints
    The app exposes a protected API under /api/reports where users can:

        View all their reports

        View a specific report

        Delete a report by ID

    Docker support
    The app is fully containerized for testing and grading. No setup beyond Docker is required.

Important Notes

    This project uses mock data only and does not perform real AWS scans.

    All findings are loaded from a JSON file located at wwwroot/mock-data/mockScanResult.json.

How to Run with Docker
1. Build the image

From the root of the project:

docker build -t security-scan .

2. Run the container

docker run -p 8080:8080 security-scan

The application will be available at:

http://localhost:8080

How to Test

    Open the application in a browser at http://localhost:8080.

    Register a new user account.

    Log in using the new account.

    Click “Create New Scan” to generate a mock security report.

    After submitting:

        You will be taken back to the list of your reports.

        You can click “Details” to view the full scan result and your note.

        You can click “Delete” to remove a report.

        You can click “Edit” from the details page to update your note.

AJAX delete and summary toggle are also available directly from the report list view.
