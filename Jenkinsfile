#!/usr/bin/env groovy

pipeline {
    agent any

    options {
        // Job timeout
        timeout(time: 1, unit: 'HOURS')
        // Add timestamps to console output
        timestamps()
        // Disable multiple builds at a time for same branch/job
        disableConcurrentBuilds()
        // Only keep 60 builds
        buildDiscarder(logRotator(numToKeepStr: '60'))
    }
	
    environment{
	MSBUILD_SONAR_HOME = tool 'SonarScanner'
    }
	
    stages {
        stage('Checkout') {
	    steps {
		print "SCM Checkout"
		checkout scm
	    }
        } 
		
	stage('Nuget_Restore') {
	    steps {
	    print "Restoring Nuget Packages on sln"
	    powershell '''
		C:\\nuget\\nuget.exe restore .\\CGP.sln 
		'''
	    }
	}
		
	stage('Build') {
            steps {
		print "Building Solution"
                powershell '''
                        if (Test-Path "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe") {
                        } else {
                            Write-Error "Cannot find VS 2019 MSBuild"
                        }
			msbuild $ENV:WORKSPACE\\CGP.sln `
			/p:DeployOnBuild=true `
			/p:Configuration=Release `
			/p:WebPublishMethod=FileSystem `
			/p:SkipInvalidConfigurations=true `
			/p:PublishUrl=$ENV:WORKSPACE\\Build_Artifacts_Jenkins `
			/p:DeployDefaultTarget=WebPublish 
                    '''
            }
        }
	    
	stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('sonarqube') {
                    withCredentials([string(credentialsId: 'sonarqube', variable: 'sonarqube')]) {
                        powershell """
                            ${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe begin /k:"project-key" /d:sonar.login=${env.sonarqube} `
			    MSBuild.exe $ENV:WORKSPACE\\CGP.sln /t:Rebuild `
                            ${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe end /d:sonar.login=${env.sonarqube} 
                        """
                    }
                }
            }
        }
	    
	stage('Removing_PDB_Files') {
            steps {
	    print "Removed .Pdb Files"
            powershell '''
		Get-ChildItem $ENV:WORKSPACE\\Build_Artifacts_Jenkins *.pdb -Recurse | foreach { Remove-Item -Path $_.FullName -Force }
                Remove-Item -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
		md -path $ENV:WORKSPACE\\Build_Package
		'''
	    }
	}
	    
    	stage('Archive_Artifacts') {
            steps {
            powershell '''
		Compress-Archive -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins `
		-DestinationPath $ENV:WORKSPACE\\Build_Package\\$ENV:BUILD_NUMBER.zip
		'''
	    }
        }
	    
	stage('Publish Artifacts to Jenkins Dashboard') {
	    steps{
                archiveArtifacts artifacts: "Build_Package\\${env:BUILD_NUMBER}.zip",  onlyIfSuccessful: true
	    }
	}    
    }
  
}
