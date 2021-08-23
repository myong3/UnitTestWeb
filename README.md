# UnitTestWeb

Run test project and create xml.
Finally, create unit testing code coverage report.

* ## 2 Methods

 ### 1. Reportgenerator

install Nuget:

1. cover.collector
2. Reportgenerator

cmd:

ReportGenerator -reports:"C:\Study\myCSharpProject\UnitTestImplementation\UnitTestImplementationTests\results\coverage.cobertura.xml" -targetdir:coveragereport  reporttypes:Html

---------------------------------------

 ### 2. MSBuild + Reportgenerator(含history xml)

install Nuget:

1. coverlet.msbuild
2. cover.collector
3. Reportgenerator


Test專案檔加入
```
  <Target Name="GenerateHtmlCoverageReport" AfterTargets="GenerateCoverageResultAfterTest">
    <ItemGroup>
      <CoverletReport Include="./results/coverage.cobertura.xml" />
    </ItemGroup>
    <ReportGenerator ReportFiles="@(CoverletReport)" TargetDirectory="./report" HistoryDirectory="./history"/>
  </Target>
```
  
cmd:

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput="./results/"
  
  Reference:
  1. https://www.dotblogs.com.tw/yc421206/Tags?qq=Unit%20Test
  2. https://docs.microsoft.com/zh-tw/dotnet/core/testing/unit-testing-code-coverage?tabs=windows
