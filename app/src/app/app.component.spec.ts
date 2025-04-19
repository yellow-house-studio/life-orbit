import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { RouterOutlet, RouterLink } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('AppComponent', () => {
  let fixture: ComponentFixture<AppComponent>;
  let component: AppComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        NoopAnimationsModule,
        RouterTestingModule,
        MatToolbarModule,
        MatButtonModule,
        MatIconModule,
        RouterLink,
        RouterOutlet,
        AppComponent
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should have the correct title', () => {
    expect(component.title).toBe('life-orbit-app');
  });

  it('should render the logo text', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const toolbar = compiled.querySelector('mat-toolbar');
    expect(toolbar).toBeTruthy();
    expect(toolbar?.querySelector('.logo')?.textContent?.trim()).toBe('LifeOrbit');
  });

  it('should render navigation buttons', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const buttons = compiled.querySelectorAll('button[mat-button]');
    expect(buttons.length).toBe(2);
    
    const buttonTexts = Array.from(buttons).map(button => 
      button.textContent?.trim().replace(/\s+/g, ' ')
    );
    expect(buttonTexts).toContain('Dashboard');
    expect(buttonTexts).toContain('Settings');
  });
});