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
	OCTOPUS_SERVER_URL = 'http://localhost:80'
	OCTOPUS_ENVIRONMENT = "Development"
	OCTOPUS_CHANNEL = "OctopusChannel"
	OCTOPUS_PROJECT = "DOTNET"
	OCTOPUS_SPACE = "Default"
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
                        powershell """
                            ${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe end `
			    /d:sonar.login=${env.sonarqube}
                        """
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
		md -path $ENV:WORKSPACE\\Build_Package
		'''
	    }
	}
	    
    	stage('Archive Artifacts') {
            steps {
            powershell '''
		Compress-Archive -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins `
		-DestinationPath $ENV:WORKSPACE\\Build_Package\\MyPackage.$ENV:BUILD_TIMESTAMP.$ENV:BUILD_NUMBER-beta.zip
		'''
	    }
        }
	    
	stage('Push Packages') {
            steps {
	       print "Pushing Package"
                withCredentials([string(credentialsId: 'Octo_API_Key', variable: 'OCTOPUS_API_KEY')]) {
		    powershell '''
			C:\\OctopusTools\\octo.exe push `
			--package $ENV:WORKSPACE\\Build_Package\\MyPackage.$ENV:BUILD_TIMESTAMP.$ENV:BUILD_NUMBER-beta.zip `
			--server $ENV:OCTOPUS_SERVER_URL `
			--apiKey $env:OCTOPUS_API_KEY 
		    '''
		}
            }
        }
		
	stage('Octopus Release') {
	    steps {
		print "Creating Release"
                withCredentials([string(credentialsId: 'Octo_API_Key', variable: 'OCTOPUS_API_KEY')]) {
                    powershell '''
			C:\\OctopusTools\\octo.exe create-release --server $ENV:OCTOPUS_SERVER_URL `
			--apikey $env:OCTOPUS_API_KEY --space $ENV:OCTOPUS_SPACE --project $ENV:OCTOPUS_PROJECT --version "$ENV:BUILD_NUMBER" `
			--channel $ENV:OCTOPUS_CHANNEL --deployto $ENV:OCTOPUS_ENVIRONMENT
		    '''
		}
            }
        }
	    
	stage('Publish Artifacts To Jenkins Dashboard') {
	    steps{
                archiveArtifacts artifacts: "Build_Package\\MyPackage.$ENV:BUILD_TIMESTAMP.$ENV:BUILD_NUMBER-beta.zip",  onlyIfSuccessful: true
	    }
	}    
    }
  
}
