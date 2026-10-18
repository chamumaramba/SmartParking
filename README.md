# SmartParking

## What this project does

SmartParking lets people book a parking space and use an access code to enter and leave. The system records the visit and works out the parking cost.

Users can pay for each visit or use a monthly subscription. The project also includes an ESP32 parking sensor and gate simulation using PlatformIO and Wokwi.

## Users and plans

- Users can create an account and sign in.
- A user profile has a username, email address, first name, and last name.
- Users can choose a pricing plan:
  - **Pay as you go:** Pay for each parking visit.
  - **Monthly subscription:** Get the parking benefits included in the active plan.
- Blocked or deactivated users cannot use the service.
- Each monthly plan must clearly say which parking lots it covers and what the subscription includes.

## Parking spaces and bookings

- The system has parking lots, and each lot has spaces.
- Every space belongs to one lot. Its slot name must be unique in that lot.
- Users can see which spaces are available and book one for a chosen time.
- The same space cannot have two confirmed bookings for overlapping times.
- Store user, parking-space, and booking data in SQLite.
- When a booking is made or changed, publish a booking event using MQTT.
- A booking is the reservation. A parking session is the actual visit, with an entry time and an exit time.

## Entry, exit, and access codes

- After a booking is made, the user receives an access code for that booking.
- The user enters the code at the entrance and uses it again at the exit.
- The gate sends entry and exit requests to the application using MQTT.
- Before opening the gate, the system checks the code, account, booking time, parking lot, and session status, then sends an allow-or-deny response over MQTT.
- A valid entry starts a parking session and saves the entry time.
- A valid exit saves the exit time, ends the session, works out the cost, and shows the result to the user.
- The system limits repeated incorrect code attempts. It must not write access codes to logs.
- Entry and exit checks use a correlated MQTT request and response so the gate can act on the result. Do not use delayed queue processing as the immediate gate response.

## Pricing and payments

- For pay-as-you-go parking, the system works out the cost when the user exits.
- If the user has an active subscription that covers the visit, the system applies its benefits.
- The system saves which price or plan was used for each visit. Later price changes must not change old charges.
- Pricing rules must explain the currency, how time is rounded, any free period, and any maximum charge.
- The amount owed and the payment result are tracked separately. A payment can be waiting, successful, failed, or refunded.
- Use a third-party payment gateway to collect pay-as-you-go charges and monthly subscription payments.
- Let the gateway handle card details through its hosted checkout or tokenized payment flow. Do not store card numbers or security codes in this project.
- Save the gateway's transaction/reference ID and the payment status, amount, currency, and related session or subscription.
- Verify signed gateway notifications (webhooks) before updating payment or subscription status. Handle retries and duplicate notifications safely.
- Use the gateway's test or sandbox mode during development. Keep its secret keys out of source control.
- If a payment fails, the system records the unpaid amount and provides a way to pay later. It must not trap a vehicle inside the lot.

## Sensors and messages

- The ESP32 gate is simulated in Wokwi with a keypad, 20x4 LCD, LEDs, and a servo.
- The keypad lets a user enter an access code. The LCD shows prompts and results, the LEDs show gate status, and the servo represents opening and closing the barrier.
- ESP32 firmware can send sensor readings and device health information.
- A sensor reading tells the system whether a space seems occupied. It does not by itself decide whether that space can be booked.
- The system checks incoming device messages and safely handles repeated or out-of-order messages.
- The device and application exchange booking events and entry/exit requests and responses using MQTT through AWS IoT Core.
- Sensor telemetry can be routed by AWS IoT Core to Amazon SQS for background processing. The gate's MQTT request and response is separate from this queued telemetry flow.

## Technologies

- **Backend:** ASP.NET Core Web API and a background worker.
- **Website:** Blazor, which uses the API to get data and perform actions.
- **Database:** SQLite with Entity Framework Core for user, parking-space, booking, and session data.
- **Accounts:** ASP.NET Core Identity. It stores password hashes, not plain-text passwords.
- **Device simulation:** ESP32 firmware built with PlatformIO and tested in Wokwi with a keypad, 20x4 LCD, LEDs, and servo.
- **Device messaging:** AWS IoT Core using MQTT over TLS.
- **Code layout:** Separate .NET projects for Domain, Application, Infrastructure, API, Worker, Web, and tests.

## Security and testing

- Users must sign in to use their account. The system checks that they can only see and change data they are allowed to access.
- Give every real device its own AWS IoT identity and only the permissions it needs.
- Keep passwords, keys, and certificates out of Git and out of Wokwi files.
- Check all information received from users and devices before using it.
- Keep access codes private. Do not put them in web addresses or logs. Store a protected check value instead of the code itself where possible.
- Save event times in UTC, and show local times when needed.
- Add tests for bookings, access checks, prices, subscriptions, repeated messages, and the full process from booking to exit.

## Decisions still to make

- Does a user choose a plan for their account, for each booking, or both?
- Do subscribers still need to book a space? Which lots does each plan cover?
- How long does an access code work, and when is it cancelled?
- What MQTT topics and message formats will be used for booking events and entry/exit requests and responses?
- What are the prices, free periods, rounding rules, and subscription benefits?
- Which third-party payment gateway will be used, and what happens when a payment is delayed or fails?
