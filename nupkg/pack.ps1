# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src/Core"

$projects = (
  "Surging.Core.ApiGateWay",
  "Surging.Core.Caching",
  "Surging.Core.Codec.MessagePack",
  "Surging.Core.Codec.ProtoBuffer",
  "Surging.Core.Common",
  "Surging.Core.Consul",
  "Surging.Core.CPlatform",
  "Surging.Core.DotNetty",
  "Surging.Core.EventBusKafka",
  "Surging.Core.EventBusRabbitMQ",
  "Surging.Core.KestrelHttpServer",
  "Surging.Core.Log4net",
  "Surging.Core.NLog",
  "Surging.Core.Protocol.Http",
  "Surging.Core.Protocol.WS",
  "Surging.Core.ProxyGenerator",
  "Surging.Core.ServiceHosting",
  "Surging.Core.Swagger",
  "Surging.Core.System",
  "Surging.Core.Zookeeper",
  "Surging.Core.Domain",
  "Surging.Core.Schedule",
  "Surging.Core.AutoMapper",
  "WebSocketCore"
)

Set-Location $slnPath
& dotnet restore Surging.sln

foreach($project in $projects) {
    $projectFolder = Join-Path $srcPath $project
    
    Set-Location $projectFolder
    Remove-Item -Recurse (Join-Path $projectFolder "bin/Release")
	& dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
	& dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true
	
	$projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder
}

Move-Item $projectPackPath $packFolder


Set-Location $packFolder