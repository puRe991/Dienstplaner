# Performance Optimization Guide

## Database Optimization

### Indexing Strategy

```csharp
modelBuilder.Entity<Shift>()
    .HasIndex(s => s.StartTime)
    .IsUnique(false);

modelBuilder.Entity<SickLeave>()
    .HasIndex(sl => new { sl.UserId, sl.StartDate })
    .IsUnique(false);

modelBuilder.Entity<Availability>()
    .HasIndex(a => new { a.UserId, a.Date })
    .IsUnique(false);
```

### Query Optimization

#### Before (N+1 Problem)
```csharp
var users = await _context.Users.ToListAsync();
foreach (var user in users)
{
    var shifts = await _context.Shifts
        .Where(s => s.AssignedUserId == user.Id)
        .ToListAsync(); // Multiple DB calls!
}
```

#### After (Optimized)
```csharp
var users = await _context.Users
    .Include(u => u.AssignedShifts)
    .ToListAsync(); // Single DB call with eager loading
```

## Caching Strategy

### Cache Layers

1. **In-Memory Cache** (L1): Hot data, short TTL (5-15 minutes)
2. **Distributed Cache** (L2): Warm data, medium TTL (1-2 hours)
3. **Database** (L3): Cold data

### Cache Keys Naming Convention

```
shifts:all
shifts:user:{userId}
shifts:unassigned
users:email:{email}
availability:user:{userId}:date:{date}
```

## API Response Optimization

### Pagination

```csharp
[HttpGet]
public async Task<IActionResult> GetShifts(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 20)
{
    var shifts = await _shiftService.GetShiftsAsync(
        pageNumber, pageSize);
    
    return Ok(shifts);
}
```

### Response Compression

```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
```

## Async/Await Best Practices

### ✅ Good
```csharp
public async Task<IEnumerable<Shift>> GetShiftsAsync()
{
    return await _context.Shifts.ToListAsync();
}
```

### ❌ Avoid
```csharp
public Task<IEnumerable<Shift>> GetShifts()
{
    return Task.FromResult(_context.Shifts.ToList());
}
```

## Connection Pool Optimization

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ShiftPilotDB;Min Pool Size=5;Max Pool Size=100;Connection Lifetime=300;"
}
```

## Monitoring and Profiling

### Enable Query Logging

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information));
```

### Performance Metrics

```csharp
var stopwatch = Stopwatch.StartNew();
// Execute operation
stopwatch.Stop();
_logger.LogInformation($"Operation took {stopwatch.ElapsedMilliseconds}ms");
```

## Load Testing

### Using Apache JMeter

1. Create test plan with:
   - Thread Group (number of users)
   - HTTP Request Sampler
   - Response Assertion

2. Run test:
   ```bash
   jmeter -n -t ShiftPilot.jmx -l results.jtl -j logs.log
   ```

### Expected Performance

- API response time: < 200ms (p95)
- Database query time: < 100ms
- Throughput: 1000+ RPS

## Scaling Strategies

### Horizontal Scaling

1. **Load Balancer** (Nginx, Azure Load Balancer)
2. **Multiple API Instances**
3. **Shared Database**
4. **Distributed Cache** (Redis)

### Vertical Scaling

1. Increase server CPU/Memory
2. Optimize database indexes
3. Implement caching
4. Use connection pooling
