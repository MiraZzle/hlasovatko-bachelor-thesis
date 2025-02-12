workspace "Hlasovátko" "Interactive voting platform for universities" {
    model {
        teacher = person "Teacher" "Creates and manages interactive activities."
        student = person "Student" "Joins sessions and participates in activities."

        hlasovatko = softwareSystem "Hlasovátko" "A web-based voting platform for real-time engagement."

        teacher -> hlasovatko "Creates activities and reviews responses"
        student -> hlasovatko "Participates in quizzes, polls, and Q&A sessions"
    }

    views {
        systemContext hlasovatko "L1_Hlasovatko" {
            include *
            autolayout lr
            description "System Context diagram for Hlasovátko"
        }

        styles {
            element "Person" {
                background #1155AA
                color white
                shape person
                fontSize 20
            }
            element "Software System" {
                background #FAC507
                color black
                shape roundedBox
                fontSize 22
            }
        }
    }

    configuration {
        scope softwaresystem
    }
}
