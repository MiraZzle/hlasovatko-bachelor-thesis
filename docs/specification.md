# Hlasovátko – Interactive Voting Platform

Hlasovátko is an interactive voting platform designed primarily for universities. The goal of this app is to enable real-time engagement between teachers and students using predefined activities like quizzes, polls, and Q&A sessions.

Unlike existing platforms like Slido or Mentimeter, Hlasovátko offers **timed activities, extended feedback sessions, and direct teacher data APIs** for analysing results. The platform prioritizes **accessibility, ease of use, and privacy**.

---

## Core Features

### Activity Types

Each activity has **configurable options** such as **anonymous responses** and **scheduled activation**.

- **Quizzes** – Supports both **timed and untimed** questions for assessing students knowledge
- **Polls & Surveys** – Collects instant feedback
- **Live Q&A** – Allows students to submit and upvote questions
- **Quick Voting** – Simple and fast method for gathering opinions
- **Predefined Activities** – Some activities (e.g., Q&A before a lecture) can remain **active for days**, enabling students to submit questions in advance and providing teachers with **early feedback**

---

## User Roles & Access Control

- **Teachers**: Create, manage, and analyze interactive activities
- **Students**: Participate easily via links or QR codes **without the need for registration**

---

## Workflow

### Teacher Workflow Example (Creating & Using a Quiz)

1. The teacher **logs into Hlasovátko** and selects **Create New Activity**
2. The teacher **chooses “Quiz”** and configures:
   - **Time limit** (optional)
   - **Anonymous mode** (optional)
   - **Question types** (multiple-choice, open-ended, etc.)
3. The system **generates a shareable QR code & link**
4. The teacher **starts the session**, the app monitors responses of students in near real time
5. The session **automatically closes** when the timer ends or manually by the teacher
6. The teacher **reviews results**, exports reports, and analyzes student performance

---

### Student Workflow Example (Joining & Answering a Quiz)

1. The student **scans the QR code or uses the link**
2. The quiz interface **opens immediately** (student may be prompted to login - depends on the anonymity mode)
3. The student **answers the questions** before the timer expires
4. The student **submits responses** (+ optionally sees **aggregated activity results** if enabled by the teacher)

---

## Responsiveness and accessability

Hlasovátko is designed for **mobile and desktop** use:

- The **UI is highly responsive**, ensuring usability across **phones, tablets, and computers**
- Each live session **generates a QR code**, making it **easy for students to join instantly**

---

## Privacy & Security

- **Anonymous responses** – Teachers can enable or disable anonymity for participants
- **Role-based access control** – Ensures that only authorized users can manage activities
- **Data security & encryption** – All responses and results are securely stored

---

## Existing Solutions Comparison

| Feature                  | Hlasovátko            | Slido       | Mentimeter     | Kahoot                 |
| ------------------------ | --------------------- | ----------- | -------------- | ---------------------- |
| Free Version Limitations | No major restrictions | 3 polls max | 50 users/month | Limited question types |
| Real-time Analytics      | Basic                 | ✅          | ✅             | ❌                     |
| Anonymity Support        | ✅                    | ✅          | ✅             | ❌                     |
| Advanced Customization   | ✅                    | ❌          | ✅             | ❌                     |
| Data API                 | ✅                    | ❌          | ❌             | ✅                     |

---

## Tech Stack

- **Frontend**: SvelteKit
- **Backend**: ASP.NET + PostgreSQL
- **Deployment**: Not decided yet
- **Authentication**: Role-based access
- **Data**: Teacher/admin accessible data APIs

---

## Roadmap

1. **Stakeholder & Requirement Analysis** (Based on student & teacher surveys)
2. **UI Wireframing and System Architecture**
3. **Core feature implementation**
4. **Testing and collectting feedback**
5. **Official launch**

---

## Potential Future Extensions

- **Gamification** – Leaderboards and rewards for engagement
- **Custom Themes** – Allowing institutions to apply branding
- **In-Platform Advanced Analytics** – More detailed insights into student performance
- **Integration with University Information Systems** – (Future consideration)
