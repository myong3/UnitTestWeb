# UnitTestWeb

## run test project and create xml ,then create code covered report.

* # 2 Methods
1. Reportgenerator

cmd:

ReportGenerator -reports:"C:\Study\myCSharpProject\UnitTestImplementation\UnitTestImplementationTests\results\coverage.cobertura.xml" -targetdir:coveragereport  reporttypes:Html

---------------------------------------

2. MSBuild + Reportgenerator(含history xml)

Test專案檔加入
```
  <Target Name="GenerateHtmlCoverageReport" AfterTargets="GenerateCoverageResultAfterTest">
    <ItemGroup>
      <CoverletReport Include="./results/coverage.cobertura.xml" />
    </ItemGroup>
    <ReportGenerator ReportFiles="@(CoverletReport)" TargetDirectory="./report" HistoryDirectory="./history"/>
  </Target>
```
  
cdm:

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput="./results/"
  
  Reference:
  1. https://www.dotblogs.com.tw/yc421206/Tags?qq=Unit%20Test
  2. https://docs.microsoft.com/zh-tw/dotnet/core/testing/unit-testing-code-coverage?tabs=windows
