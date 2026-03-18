# RentAPlace Deployment Guide

## Local Development

1. Run main API:
   `dotnet run --project Backend/RentAPlace.API`
2. Run messaging API:
   `dotnet run --project Backend/RentAPlace.Messaging.API`
3. Run Angular frontend:
   `cd Frontend && npm start`

## SMTP Setup

Configure these values in `Backend/RentAPlace.API/appsettings.Development.json`:

- `Email:SmtpHost`
- `Email:SmtpPort`
- `Email:SenderEmail`
- `Email:Username`
- `Email:Password`
- `Email:UseSsl`

Booking creation and status updates will trigger email notifications.

## Docker Deployment

From repository root:

1. `docker compose build`
2. `docker compose up -d`
3. API: `http://localhost:5255/swagger`
4. Messaging API: `http://localhost:5256/swagger`
5. Frontend: `http://localhost:4200`

## CI

GitHub Actions workflow is located at:
`.github/workflows/ci.yml`

It runs backend restore/build/test and frontend build on push and pull request.
