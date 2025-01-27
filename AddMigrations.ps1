param (
    [string]$Name
)

if (-not $Name) {
    Write-Host "Error: Provide a migration name." -ForegroundColor Red
    exit
}

dotnet ef migrations add $Name --project "OrderPaymentSystem.Orders.Domain" --startup-project "OrderPaymentSystem.Orders.Web"
