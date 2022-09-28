@echo on
for /d %%f in (..\PumpFacts\*) do call :remove %%f
exit /b

:remove
rd "%1\bin" /s /q
rd "%1\obj" /s /q
exit /b
