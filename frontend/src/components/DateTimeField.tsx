import * as React from 'react';
import dayjs, { Dayjs } from 'dayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import styled from "styled-components";
import CalendarIcon from './CalendarIcon'
import { observer } from 'mobx-react';
import 'dayjs/locale/ru';

type DateTimePickerValueProps = {
  id: string;
  value: Dayjs;
  label: string;
  name: string;
  minDateTime?: Dayjs;
  maxDateTime?: Dayjs;
  disabled?: boolean;
  error?: boolean;
  helperText: string;
  onChange: (e) => void;
};


const DateTimeField: React.FC<DateTimePickerValueProps> = observer((props) => {
  return (
    <div id={props.id} data-testid={props.id} >
      <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="ru">
        <WrappedTimeField
          format="DD.MM.YYYY HH:mm"
          value={props.value}
          label={props.label}
          disabled={props.disabled}
          ampm={false}
          onChange={(newValue: Dayjs) => {
            if (newValue instanceof dayjs || newValue === null) {
              props.onChange({ target: { value: newValue, name: props.name } })
            }
          }}
          closeOnSelect={false}
          slotProps={{
            textField: {
              helperText: props.helperText,
              error: props.error,
            },
          }}
          minDateTime={props.minDateTime}
          maxDateTime={props.maxDateTime}
          slots={{
            openPickerIcon: CalendarIcon
          }}
        />
      </LocalizationProvider>
    </div>
  );
})

const WrappedTimeField = styled(DateTimePicker)`
  width: 100% !important;
  input {
    padding: 9px 8px;
    border-color: #CDD3EC;
  }
  label {
    position: absolute;
    top: -8px;
  }
`
export default DateTimeField;