param (
	$buildPath,
	$outputPath,
	$name
)

$target = @{
	Path = $buildPath + "*"
	CompressionLevel = "Fastest"
	DestinationPath = (Join-Path -Path $outputPath -ChildPath "$($name).zip")
}

Compress-Archive @target