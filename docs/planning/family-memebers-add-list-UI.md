# Family Members UI Implementation Plan

## 📋 Overview

Implementation of the UI for managing family members in the LifeOrbit application, focusing on listing and adding new family members.

## 🏗️ Architecture

### Directory Structure
```
app/src/app/
├── infrastructure/
│   └── family/
│       ├── models/
│       │   ├── family.model.ts
│       │   ├── family.dto.ts
│       │   └── family.mapper.ts
│       ├── services/
│       │   └── family-api.service.ts
│       └── stores/
│           └── family-members.store.ts
└── features/
    └── settings/
        ├── pages/
        │   ├── family-member-dashboard/
        │   │   └── family-member-dashboard.page.ts
        │   └── add-family-member/
        │       └── add-family-member.page.ts
        ├── containers/
        │   └── manage-family/
        │       └── manage-family.component.ts
        └── components/
            ├── allergy-list/
            │   └── allergy-list.component.ts
            ├── safe-foods-list/
            │   └── safe-foods-list.component.ts
            └── food-preferences-list/
                └── food-preferences-list.component.ts
```

## 📝 Implementation Details

### 1. Models & DTOs

#### Application Models (family.model.ts)
```typescript
export interface FamilyMember {
  id: string;
  name: string;
  age: number;
  allergies: Allergy[];
  safeFoods: SafeFood[];
  foodPreferences: FoodPreference[];
}

export interface Allergy {
  allergen: string;
  severity: AllergenSeverity;
}

export enum AllergenSeverity {
  AvailableForOthers = 'AvailableForOthers',
  NotAllowed = 'NotAllowed'
}

// Similar interfaces for SafeFood and FoodPreference
```

#### DTOs (family.dto.ts)
```typescript
export interface FamilyMemberResponseDto {
  id: string;
  name: string;
  age: number;
  allergies: AllergyDto[];
  safeFoods: SafeFoodDto[];
  foodPreferences: FoodPreferenceDto[];
}

export interface CreateFamilyMemberRequestDto {
  name: string;
  age: number;
}
```

### 2. Form Validation Rules

#### Name Validation
- Required
- Min length: 2 characters
- Max length: 100 characters
- Pattern: Only letters, spaces, and hyphens

#### Age Validation
- Required
- Min value: 0
- Max value: 120
- Must be a whole number

### 3. Components Breakdown

#### Manage Family Component
- Displays list of family members in grid/list view
- Each member card shows:
  - Name
  - Age
  - Quick stats (number of allergies, safe foods, preferences)
- Add button in prominent position
- Loading state handling
- Error state handling

#### Add Family Member Page
- Stepper form with sections:
  1. Basic Info (name, age)
  2. Allergies
  3. Safe Foods
  4. Food Preferences
- Progress indicator
- Save & Cancel buttons
- Form validation feedback
- Success/Error notifications

#### Dumb Components

##### Allergy List Component
- Input field for allergen
- Severity selector (radio buttons)
- Add/Remove functionality
- Validation for duplicates

##### Safe Foods List Component
- Input field for food items
- Add/Remove functionality
- Validation for duplicates
- Chips/tags display

##### Food Preferences List Component
- Input field for food item
- Status selector (radio buttons)
- Add/Remove functionality
- Validation for duplicates

### 4. Store Implementation

```typescript
interface FamilyMembersState {
  familyMembers: FamilyMember[];
  loading: boolean;
  error: string | null;
}

// Actions
- loadFamilyMembers()
- addFamilyMember(member: CreateFamilyMemberRequestDto)
```

### 5. Routing

```typescript
const routes: Routes = [
  {
    path: 'settings/family',
    children: [
      {
        path: '',
        component: ManageFamilyComponent
      },
      {
        path: 'add',
        component: AddFamilyMemberPage
      },
      {
        path: ':id',
        component: FamilyMemberDashboardPage
      }
    ]
  }
];
```

## 🧪 Testing Strategy

### Unit Tests
- Models: Type validation
- Mappers: Conversion accuracy
- Store: State management
- Components: Rendering and interactions
- Forms: Validation rules

### Integration Tests
- Store + API Service
- Form submission flow
- Navigation between components

### E2E Tests (Future)
- Complete add family member flow
- List view interactions
- Navigation between views

## 📱 Responsive Design

- Mobile-first approach
- Breakpoints:
  - < 768px: Single column layout
  - >= 768px: Grid layout for member list
  - >= 1024px: Enhanced grid layout with more details

## 🎨 UI/UX Considerations

### Loading States
- Skeleton loaders for member list
- Progress indicators for form submission

### Error Handling
- Inline form validation messages
- Toast notifications for API errors
- Retry mechanisms for failed operations

### Accessibility
- ARIA labels
- Keyboard navigation
- High contrast support
- Screen reader friendly form validation

## 📅 Implementation Order

1. Set up infrastructure (models, DTOs, mappers)
2. Implement API service
3. Create store with basic functionality
4. Build manage-family component
5. Implement add form components
6. Add validation
7. Connect everything with store
8. Write tests
9. Polish UI/UX

## 🔄 Future Enhancements (Not in Current Scope)

- Edit functionality
- Bulk operations
- Advanced filtering/sorting
- Family member categories/groups
- Import/Export functionality 