import { TestBed, async } from '@angular/core/testing';

import { AppComponent } from '../app/app.component';
import { capturePage } from './test-util';

describe('AppComponent', () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [
          AppComponent
        ],
      }).compileComponents();
    }));
    it('Launches the app', async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
    
// describe('Set treatment', () => {
//     it('should launch browser ', async(() => {
//         const browser = puppeteer.launch();
//         expect(false).toBeTruthy("ssssss");
        
//         // const page = browser.newPage();
//         // page.goto('https://example.com');
//         // page.screenshot({ path: 'example.png' });

//        // browser.close();



//         expect(false).toBeTruthy();
//     }));

//     it(`should pass`, async(() => {
//         expect(true).toBeFalsy();
//     }));
});
