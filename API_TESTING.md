# API Testing Guide

## Prerequisites

- Postman or any REST client (Thunder Client, Insomnia, etc.)
- Running ShiftPilot API server

## Authentication

### 1. Register a New User

**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
  "email": "employee@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Registration successful"
}
```

### 2. Login

**Endpoint:** `POST /api/auth/login`

**Request Body:**
```json
{
  "email": "employee@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login successful"
}
```

## Using JWT Token

Add the token to the Authorization header for all subsequent requests:

```
Authorization: Bearer <your_jwt_token>
```

## API Endpoints Testing

### Shifts

#### Create a Shift

**Endpoint:** `POST /api/shifts`

**Request Body:**
```json
{
  "startTime": "2026-05-25T08:00:00Z",
  "endTime": "2026-05-25T16:00:00Z",
  "type": 4,
  "notes": "Full day shift"
}
```

#### Get All Shifts

**Endpoint:** `GET /api/shifts`

#### Get Unassigned Shifts

**Endpoint:** `GET /api/shifts/unassigned`

#### Assign Shift to User

**Endpoint:** `POST /api/shifts/{shiftId}/assign/{userId}`

### Sick Leave

#### Create Sick Leave Request

**Endpoint:** `POST /api/sickleaves`

**Request Body:**
```json
{
  "userId": 1,
  "startDate": "2026-05-25T00:00:00Z",
  "endDate": "2026-05-26T00:00:00Z",
  "notes": "Flu"
}
```

#### Approve Sick Leave

**Endpoint:** `POST /api/sickleaves/{id}/approve`

#### Reject Sick Leave

**Endpoint:** `POST /api/sickleaves/{id}/reject`

### Shift Swaps

#### Create Swap Request

**Endpoint:** `POST /api/shiftswaps`

**Request Body:**
```json
{
  "initiatorId": 1,
  "targetUserId": 2,
  "initiatorShiftId": 5,
  "targetShiftId": 6,
  "reason": "Personal appointment"
}
```

#### Approve Swap Request

**Endpoint:** `POST /api/shiftswaps/{id}/approve`

#### Reject Swap Request

**Endpoint:** `POST /api/shiftswaps/{id}/reject`

### Availability

#### Create Availability

**Endpoint:** `POST /api/availability`

**Request Body:**
```json
{
  "userId": 1,
  "date": "2026-05-25T00:00:00Z",
  "startTime": "08:00",
  "endTime": "17:00",
  "isAvailable": true,
  "reason": null
}
```

#### Get Availabilities for Date Range

**Endpoint:** `GET /api/availability/range?startDate=2026-05-25&endDate=2026-05-31`

### Scheduling

#### Generate Weekly Schedule

**Endpoint:** `POST /api/scheduling/generate-weekly-schedule?weekStart=2026-05-25`

#### Get Conflicts

**Endpoint:** `GET /api/scheduling/conflicts/{userId}`

#### Get Available Shifts for User

**Endpoint:** `GET /api/scheduling/available-shifts/{userId}?startDate=2026-05-25&endDate=2026-05-31`

## Swagger UI

Once the API is running, visit:
```
https://localhost:5001/swagger
```

All endpoints are documented and can be tested directly from the Swagger UI.
