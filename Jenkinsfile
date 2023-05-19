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
	key = "jenkins"
  }
	
  stages {
    stage('Checkout') {
	  steps {
		print "SCM Checkout"
		checkout scm
	  }
    } 
	stage('Nuget Restore') {
	  steps {
	    print "Restoring Nuget Packages on sln"
	    powershell '''
		C:\\nuget\\nuget.exe restore .\\CGP.sln 
		'''
	  }
	}    
	stage('Start Sonarqube Scanner') {
	  steps {
		print "Sonarqube Analysis Start"
        withSonarQubeEnv('sonarqube') {
          withCredentials([string(credentialsId: 'sonarqube', variable: 'sonarqube')]) {
            powershell """
            ${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe begin `
			/k:${env.key} `
			/d:sonar.login=${env.sonarqube} `
			/d:sonar.host.url=http://localhost:9000/
			"""
		  }
		}
	  }
	}
	stage('Build Solution') {
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
		print "Sonarqube Analysis End"
        withSonarQubeEnv('sonarqube') {
          withCredentials([string(credentialsId: 'sonarqube', variable: 'sonarqube')]) {
          powershell '''
          ${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe end `
		  /d:sonar.login=${env.sonarqube}
          '''
          }
        }
      }
    }	    
	stage('Remove PDB Files') {
      steps {
	    print "Removed .Pdb Files"
        powershell '''
		Get-ChildItem $ENV:WORKSPACE\\Build_Artifacts_Jenkins *.pdb -Recurse | foreach { Remove-Item -Path $_.FullName -Force }
        Remove-Item -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
		md -path $ENV:WORKSPACE\\Build_Package.$ENV:BUILD_NUMBER
		'''
	  }
	}	    
	stage('Publish Artifacts To Jenkins Dashboard') {
	  steps{
		archiveArtifacts artifacts: "Build_Package.$ENV:BUILD_NUMBER",  onlyIfSuccessful: true
	  }
	}    
  }
}
