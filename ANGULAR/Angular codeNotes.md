
---------Event Emitter-----------

  import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

  @Output() setJogfalse: EventEmitter<any> = new EventEmitter();

  this.setJogfalse.emit(jogCue.axes[event.previousIndex]);

  <div class="jog-model" *ngIf="showOrHideJogModel">
    <app-jog (jogModel)="hideJogModel($event)" (refreshHomeAllAxis)="fetchAxis()" (setJogfalse)="setJogfalse($event)"
      [updateCues]="refreshJogComponent"></app-jog>
  </div>


----------------  Delay-----------
//meathod

  waitFor() {
    return new Promise((resolve) => setTimeout(resolve, 1000));
  }
  

//calling  await this.waitFor();


--------------onclickpage routing to next page--- routerLink---------------

 <div class="pt-5" *ngIf="!HideSplashScreen" routerLink="/login"></div>




-------------ElectronService------------------


import { Injectable } from '@angular/core';

import { ipcRenderer, webFrame } from 'electron';
import * as remote from '@electron/remote';
import * as childProcess from 'child_process';
import * as fs from 'fs';
import * as SerialPort from 'serialport';

@Injectable({
  providedIn: 'root'
})
export class ElectronService {
  ipcRenderer: typeof ipcRenderer;
  webFrame: typeof webFrame;
  remote: typeof remote;
  childProcess: typeof childProcess;
  fs: typeof fs;
  serialPort: typeof SerialPort;

  get isElectron(): boolean {
    return !!(window && window.process && window.process.type);
  }

  constructor() {
    // Conditional imports
    if (this.isElectron) {
      this.ipcRenderer = window.require('electron').ipcRenderer;
      this.webFrame = window.require('electron').webFrame;

      this.childProcess = window.require('child_process');
      this.fs = window.require('fs');

      // If you want to use a NodeJS 3rd party deps in Renderer process (like @electron/remote),
      // it must be declared in dependencies of both package.json (in root and app folders)
      // If you want to use remote object in renderer process, please set enableRemoteModule to true in main.ts
      this.remote = window.require('@electron/remote');

      // communicate with serial port
      this.serialPort = window.require('serialport');
    }
  }

  shutDown() {
    window.require('electron-shutdown-command').shutdown();
  }
}


--------------Spinner-------------------------

<div class="spinner-wrapper" *ngIf="spinner">
    <div class="spinner">
        <i class="fa-solid fa-spinner fa-spin-pulse spinner-icon"></i>
        <h3 class="spinner-text">Please wait...</h3>
    </div>
</div>


------------icon fixed at a positin---------

 <a class="position-absolute icon-poweroff text-white"
  (click)="">
  <i class="fa-light fa-power-off"></i>
 </a>

.icon-poweroff {
    right: 56px;
    bottom: 24px;
    font-size: 26px;
}


---------Export sheet to Excel-------------

  exportEventLogs() {
    //common code
    const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const worksheet = XLSX.utils.json_to_sheet(this.eventLogs);

    //json to excel
    const workbook = {
      Sheets: {
        'testingsheet': worksheet
      },
      SheetNames: ['testingsheet']
    };

    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

    // to download
    const blobData = new Blob([excelBuffer], { type: EXCEL_TYPE });
    saveAs(blobData, 'EventsLog');
  }
}


-------------Disabled----------button--------

 <button type="button" class="btn btn-primary ms-3" [disabled]="!group.groupName || !group.groupType";

---------Ternary Operator----------------

DisplayName = (showID > 0 && string.IsNullOrEmpty(r.DisplayName) == false) ? r.DisplayName : g.AxisName,
    <span [ngClass]="{ 'current-text-color': groupItem.current > groupItem.offset }"> {{ IsAxisOffline ? 'Offline' : (calculateCurrent(groupItem.position, groupItem.offset, i) | number)}}</span>


---------Logic to Filter Duplicate -----------

     let num = this.cueList.length +1;
        let duplicate:boolean=true;
        let tempa:any=0;
        let tempb:any=0;
        while(duplicate){
          let match = this.cueList.findIndex(T => T.cueName == 'cue ' + num);
          if(match != -1){
            num ++;
            cue.cueName = 'cue ' +  num;
            cue.cueNumber = num;
            tempa++;
          }
          tempb++;
          if(tempa!= tempb){
              duplicate = false;
          }
        }


-----------Get Clock--------------------
import { map, share } from 'rxjs/operators';
import { Observable, Subscription, timer } from 'rxjs';


  rxTime :any;
  intervalId;
  subscription: Subscription;



    this.subscription = timer(0, 1000)
    .pipe(
      map(() => this.datepipe.transform((new Date) , 'MM/dd/yyyy h:mm:ss a' )),
      share()
    )
    .subscribe(time => {
      this.rxTime = time;
    });
  }

  ngOnDestroy() {
    clearInterval(this.intervalId);
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

# ----------calling function continuously ---------

      async ngOnInit(): Promise<void>
      {
      await this.refreshModelData();    
      }

  async refreshModelData() {
    this.observerSubs = new Observable((observer) => {
      this.interval = setInterval(() => {
        observer.next();
      }, 1000);
    });

    this.observerSubs.subscribe(async (res) => {
      await this.getAxisDataById();

      if (this.interval && this.modelIsOpen) {
        clearInterval(this.interval);
      }
    });
  }

-----------Tofixed ---------without Rounding--------
 toFixed(num, fixed) {
    fixed = fixed || 0;
    fixed = Math.pow(10, fixed);
    return Math.floor(num * fixed) / fixed;

------------  function in Html Return value which we are using in multiple ternary operator------------

<path id="Load_Value_100" fill="#FBB03B"
        [ngClass]=" calculate(axis) == 0 ? 'loadgrey' : calculate(axis) == 1 ? 'loadyeallow' :'loadred'"
        d="M99.1,120.6l1.1,2c0.9-0.5,1.8-1,2.7-1.5l-1.2-1.9C100.8,119.7,99.9,120.2,99.1,120.6z" />

------------------how to open a link in electron app
Make sure you have Electron properly integrated into your Angular project. 
You may need to install the electron module using npm or yarn and configure
your project's main.js file to launch the Electron app.

1step:
import { shell } from "electron";
import { ElectronService } from '../../../core/services';

2step:
constructor(private dialog: MatDialog,
private electron: ElectronService) { }

3rd step:
  showMomData(ip: any) {
    if(this.electron.isElectron)
    {
    const a = "https://";
    const b = a + ip;
    const url = b + "/Tc3PlcHmiWeb/Port_851/Visu/kid.htm";
    shell.openExternal(url);
    }
  }

------------------how to Autoopen an GUI application in Raspberry App--------
1. open Terminal and run these comands
set any  revelant name in place of xyz
mkdir /home/pi/.config/autostart
nano /home/pi/.config/autostart/xyz.desktop

2. 
[Desktop Entry]
Type=Application
Name= xyz
Exec= application name => see in the folder where all apps are shown .
Save and exit with ctrl + x, followed by y when prompted to save, and then enter. Reboot with:sudo reboot

------- To Remove the app from autostart----

rm /home/pi/.config/autostart/xyz.desktop

-------------Conver Values into another bw two numbers-------------
  //   NewValue = NewRangeMin + (NewRangeMax - NewRangeMin) * ((OldValue - OldRangeMin) / (OldRangeMax - OldRangeMin))

  // In your case:

  // NewRangeMin is -165,170
  // NewRangeMax is 165,-170
  // OldRangeMin is 1000,1000
  // OldRangeMax is 1255,1255


// to detect a change in a variable

1 we declared a private variable initialMyVariable and set its initial value, it could be string ,boolean, number etc
2 we pass initialvalue into variablw for which we want to detect change
3 we check if initial value is changed if yes we change the initial value and then we write any logic upn change.

  private initialMyVariable: string = null;
  showName: string = this.initialMyVariable;


  ngDoCheck(): void {
    // Check for changes
    if (this.showName !== this.initialMyVariable) {
     this.initialMyVariable = this.showName;
      console.log(this.showName);
      this.fetchAxis();
    }
  }

//TO Auto Scroll to a element

Html

1.<span (click)="scroll('.cue-male-icon')" class="ps-2">SHOWS</span>

2.<div class="cue-male-icon"</div>

Ts


  scroll(className: string):void
  {
    const elementList = document.querySelectorAll(className);
    const element = elementList[0] as HTMLElement;
    element.scrollIntoView({ block: 'start' });
  }
