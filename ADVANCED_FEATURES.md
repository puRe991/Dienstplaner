# Advanced Features Guide

## Reporting System

Generate comprehensive reports for business intelligence and analytics.

### Shift Reports

```bash
GET /api/reports/shifts?startDate=2026-05-01&endDate=2026-05-31
```

Returns:
- Shifts by type (Morning, Afternoon, Evening, Night, FullDay)
- Assignment rates
- Unassigned shifts count

### Sick Leave Reports

```bash
GET /api/reports/sick-leaves?startDate=2026-05-01&endDate=2026-05-31
```

Returns:
- Sick leave requests by status (Pending, Approved, Rejected)
- Total sick days taken
- Trends and patterns

### Employee Performance Reports

```bash
GET /api/reports/employee-performance/{userId}?startDate=2026-05-01&endDate=2026-05-31
```

Returns:
- Employee name and ID
- Total shifts worked
- Shift swap statistics
- Sick days taken
- Reliability score

## Caching System

Improve API performance with intelligent caching.

### Example Usage

```csharp
private readonly ICacheService _cacheService;

public async Task<IEnumerable<Shift>> GetShiftsAsync()
{
    const string cacheKey = "all_shifts";
    
    var cached = await _cacheService.GetAsync<List<Shift>>(cacheKey);
    if (cached != null)
        return cached;
    
    var shifts = await _shiftRepository.GetAllShiftsAsync();
    await _cacheService.SetAsync(cacheKey, shifts.ToList(), TimeSpan.FromHours(1));
    
    return shifts;
}
```

## Audit Logging

Track all important actions in the system.

### Logged Actions

- User registration and login
- Shift assignments and modifications
- Sick leave requests
- Shift swap approvals/rejections
- Admin actions

### Example

```csharp
await _auditService.LogActionAsync(
    userId: 1,
    action: "SHIFT_ASSIGNED",
    details: "User assigned to shift ID 5",
    changesBefore: null,
    changesAfter: "{ status: 'Assigned' }"
);
```

## Email Notifications

### Setup SMTP Configuration

Update `appsettings.json`:

```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderEmail": "noreply@shiftpilot.com",
  "SenderPassword": "your-app-password"
}
```

### Send Notifications

```csharp
// Shift assignment
await _emailService.SendShiftNotificationAsync(
    userEmail: "employee@example.com",
    shiftDetails: "Morning shift on 2026-05-25 from 08:00 to 16:00"
);

// Sick leave status
await _emailService.SendSickLeaveNotificationAsync(
    userEmail: "employee@example.com",
    status: "approved"
);
```

## Rate Limiting

Protect the API from abuse with intelligent rate limiting.

### Configured Limits

- **Fixed Window**: 100 requests per minute
- **Sliding Window**: 50 requests per 10 seconds

### Example

```csharp
[HttpGet]
[RateLimiterPolicy("fixed")]
public async Task<IActionResult> GetAllShifts()
{
    // Returns 429 Too Many Requests if limit exceeded
}
```

## Admin Dashboard

Get an overview of the system at a glance.

```bash
GET /api/admin/dashboard
```

Returns:
- Total employees
- Shift statistics
- Sick leave overview
- System health metrics

## Best Practices

1. **Use Caching** for frequently accessed data
2. **Enable Auditing** for compliance and security
3. **Monitor Performance** using built-in logging
4. **Set Up Email** for user notifications
5. **Implement Rate Limiting** to protect your API
6. **Generate Reports** for business insights
