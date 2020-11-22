param (
	$basePath
)

$path = Join-Path -Path $basePath appsettings.Development.json
Remove-Item $path