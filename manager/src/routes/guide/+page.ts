/** @type {import('./$types').PageLoad} */
export const load = () => {
	// Possible API call to get the schemas??
	const activitySchemas = [
		{
			type: 'Multiple Choice',
			schema: `{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "$id": "https://yourdomain.com/schemas/multiple_choice.json",
  "title": "Multiple Choice Activity",
  "description": "Schema for a multiple-choice question activity.",
  "type": "object",
  "properties": {
    "options": {
      "description": "The list of choices available for the question.",
      "type": "array",
      "minItems": 1,
      "items": {
        "type": "object",
        "properties": {
          "id": {
            "type": "string",
            "description": "A unique identifier for the option."
          },
          "text": {
            "type": "string",
            "description": "The display text for the option."
          }
        },
        "required": [ "id", "text" ]
      }
    },
    "allowMultipleAnswers": {
      "description": "Whether the user can select more than one option.",
      "type": "boolean",
      "default": false
    },
    "correctOptionId": {
      "description": "The ID(s) of the correct option(s). Can be a single ID or an array of IDs if multiple answers are allowed.",
      "oneOf": [
        { "type": "string" },
        {
          "type": "array",
          "items": { "type": "string" }
        }
      ]
    }
  },
  "required": [ "options" ]
}`
		},
		{
			type: 'Poll',
			schema: `{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "$id": "https://yourdomain.com/schemas/poll.json",
  "title": "Poll Activity",
  "description": "Schema for a simple poll with multiple options.",
  "type": "object",
  "properties": {
    "options": {
      "description": "The list of choices available for the poll.",
      "type": "array",
      "minItems": 1,
      "items": {
        "type": "object",
        "properties": {
          "id": {
            "type": "string",
            "description": "A unique identifier for the option."
          },
          "text": {
            "type": "string",
            "description": "The display text for the option."
          }
        },
        "required": [ "id", "text" ]
      }
    }
  },
  "required": [ "options" ]
}`
		},
		{
			type: 'Scale Rating',
			schema: `{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "$id": "https://yourdomain.com/schemas/scale_rating.json",
  "title": "Scale Rating Activity",
  "description": "Schema for an activity where users provide a rating on a numerical scale.",
  "type": "object",
  "properties": {
    "min": {
      "description": "The minimum value of the rating scale.",
      "type": "integer"
    },
    "max": {
      "description": "The maximum value of the rating scale. Should be greater than min.",
      "type": "integer"
    },
    "minLabel": {
      "description": "An optional label for the minimum value (e.g., 'Confused').",
      "type": "string"
    },
    "maxLabel": {
      "description": "An optional label for the maximum value (e.g., 'Confident').",
      "type": "string"
    }
  },
  "required": [ "min", "max" ]
}`
		},
		{
			type: 'Open-Ended',
			schema: `{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "$id": "https://yourdomain.com/schemas/open_ended.json",
  "title": "Open-Ended Activity",
  "description": "Schema for an activity where users provide a text-based answer.",
  "type": "object",
  "properties": {
    "placeholder": {
      "description": "Optional placeholder text to display in the input field.",
      "type": "string"
    }
  },
  "additionalProperties": false
}`
		}
	];

	return {
		activitySchemas: activitySchemas
	};
};
