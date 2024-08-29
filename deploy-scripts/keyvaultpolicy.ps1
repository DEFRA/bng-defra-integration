param(
    [Parameter(Mandatory = $true)]
    [string]$KeyVaultName,
    [Parameter(Mandatory = $true)]
    [string]$FunctionAppName
)
$FunctionAppObjID = Get-AzADServicePrincipal -DisplayName $FunctionAppName

$AddKeyVaultAccessPolicyParams = @{
    VaultName            = $KeyVaultName
    ObjectId             = ($FunctionAppObjID).Id
    PermissionsToSecrets = Get
}
Set-AzKeyVaultAccessPolicy @AddKeyVaultAccessPolicyParams