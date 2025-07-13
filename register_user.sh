#!/bin/bash

# API URL for registration endpoint
API_URL="http://localhost/server/api/v1/auth/register"

# Display usage information for the script
show_help() {
  cat << EOF
Usage: $0 "User Name" <email>
Registers a new user for the EngaGenie platform by prompting for a password.

Arguments:
  "User Name"   The full name of the user (must be enclosed in quotes).
  email         The user's email address.

Options:
  -h, --help    Display this help message and exit.

Example:
  $0 "Clovek Uzasny" "clovek.uzasny@example.com"
EOF
}

# Check for help flag or incorrect number of args
if [[ "$1" == "-h" || "$1" == "--help" ]]; then
  show_help
  exit 0
fi

if [ "$#" -ne 2 ]; then
  show_help
  exit 1
fi

NAME=$1
EMAIL=$2

# Prompt for the password securely
read -sp "Enter Password: (at least 6 chars long): " PASSWORD
echo

JSON_PAYLOAD=$(cat <<EOF
{
  "Name": "$NAME",
  "Email": "$EMAIL",
  "Password": "$PASSWORD"
}
EOF
)

# Send the curl request
echo "Trying to register user: $NAME <$EMAIL>"
curl -s -X POST \
     -H "Content-Type: application/json" \
     -d "$JSON_PAYLOAD" \
     "$API_URL"

echo