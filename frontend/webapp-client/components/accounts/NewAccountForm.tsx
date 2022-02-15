import Image from 'next/image';
import classnames from 'classnames';
import styles from '../../styles/Form.module.scss';
import { AddIcon, EyeIcon, EditIcon, DeleteBinIcon } from '../icons';

import {
  cloneElement,
  createRef,
  ReactElement,
  ReactNode,
  useEffect,
  useState,
} from 'react';

import {
  FieldError,
  FormProvider,
  SubmitHandler,
  useFieldArray,
  useForm,
  useFormContext,
} from 'react-hook-form';

interface NewAccountFormProps {
  className?: string;
}

type NewAccountFormValues = {
  title: string;
  code?: string;
  type: number;
  notes?: { text: string }[];
  tags?: { name: string }[];
};

const NewAccountForm = ({ className }: NewAccountFormProps) => {
  const formMethods = useForm<NewAccountFormValues>();
  const { handleSubmit } = formMethods;

  const onSubmitAccount: SubmitHandler<NewAccountFormValues> = async (data) => {
    const url = process.env.NEXT_PUBLIC_BORING_FINANCES_URL;

    const payload = {
      title: data.title,
      code: data.code,
      accountTypeId: data.type,
      notes: data.notes?.map((x) => x.text),
      tags: data.tags?.map((x) => x.name),
    };

    const response = await fetch(`${url}/api/accounts`, {
      method: 'POST',
      body: JSON.stringify(payload),
      headers: {
        'Accept': 'application/json, text/plain, */*',
        'Content-Type': 'application/json',
      },
    });

    const createdAccount = await response.json();

    console.log(response.status, createdAccount);
  };

  return (
    <FormProvider {...formMethods}>
      <div className={classnames(styles.form, className)}>
        <form
          id="new-account"
          onSubmit={handleSubmit(onSubmitAccount)}
          hidden
        />
        <header>
          <h2>New Account</h2>
          <hr />
        </header>
        <section className={styles.container}>
          <GeneralSection />
          <AnnotationsSection />
        </section>
        <footer>
          <button
            form="new-account"
            type="submit"
            className={`${styles.button} ${styles.submit}`}
          >
            Create
          </button>
        </footer>
      </div>
    </FormProvider>
  );
};

const GeneralSection = () => {
  return (
    <section className={styles.propertySection}>
      <header>
        <h3>General</h3>
        <p>Details about the account reference and domain.</p>
      </header>
      <div className={styles.container}>
        <PictureProperty />
        <TitleProperty />
        <CodeProperty />
        <TypeProperty />
      </div>
    </section>
  );
};

const AnnotationsSection = () => {
  return (
    <section className={styles.propertySection}>
      <header>
        <h3>Annotations</h3>
        <p>Metadata for custom querying and information.</p>
      </header>
      <div className={styles.container}>
        <NotesProperty />
        <TagsProperty />
      </div>
    </section>
  );
};

const PictureProperty = () => {
  return (
    <section className={`${styles.property} ${styles.picture}`}>
      <div className={classnames(styles.container, styles.main)}>
        <div className={styles.frame}>
          <Image
            className={styles.image}
            src="/piggy.jpg"
            alt="Piggy bank eating coins. Photo by @andretaissin on Unplash."
            width={64}
            height={64}
            layout="fixed"
            objectFit="cover"
          />
        </div>
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        <p>Additional Message</p>
      </div>
    </section>
  );
};

const TitleProperty = () => {
  const {
    register,
    formState: { errors },
  } = useFormContext<NewAccountFormValues>();

  const field = register('title', { required: 'Title is required' });
  const label = <label htmlFor={field.name}>Title</label>;
  const input = (
    <input
      {...field}
      form="new-account"
      autoComplete="off"
      placeholder="Untitled"
      className={classnames(styles.input, styles.simple)}
    />
  );

  return (
    <SimplePropertyLayout
      label={label}
      input={input}
      fieldError={errors.title}
    />
  );
};

const CodeProperty = () => {
  const {
    register,
    formState: { errors },
  } = useFormContext<NewAccountFormValues>();

  const field = register('code');
  const label = <label htmlFor={field.name}>Code</label>;
  const input = (
    <input
      {...field}
      form="new-account"
      autoComplete="off"
      placeholder="Optional..."
      className={classnames(styles.input, styles.simple)}
    />
  );

  return (
    <SimplePropertyLayout
      label={label}
      input={input}
      fieldError={errors.code}
    />
  );
};

const TypeProperty = () => {
  const {
    register,
    formState: { errors },
  } = useFormContext<NewAccountFormValues>();

  const field = register('type', {
    required: 'Type is required',
    min: { value: 1, message: 'Type is required' },
  });

  const label = <label htmlFor={field.name}>Type</label>;
  const input = (
    <select
      {...field}
      form="new-account"
      className={classnames(styles.input, styles.simple)}
    >
      <option value={0}>Select...</option>
      <option value={1}>Equity</option>
      <option value={2}>Asset</option>
      <option value={3}>Liability</option>
      <option value={4}>Income</option>
      <option value={5}>Expense</option>
      <option value={6}>Exchange</option>
    </select>
  );

  return (
    <SimplePropertyLayout
      label={label}
      input={input}
      fieldError={errors.type}
    />
  );
};

interface SimplePropertyLayoutProps {
  label?: ReactElement;
  input: ReactElement;
  fieldError?: FieldError;
}

const SimplePropertyLayout = ({
  label,
  input,
  fieldError,
}: SimplePropertyLayoutProps) => {
  const finalInput = cloneElement(input, {
    'aria-invalid': fieldError ? true : false,
  });
  return (
    <section className={styles.property}>
      <div className={classnames(styles.container, styles.main)}>
        {label}
        {finalInput}
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        {fieldError && <p className={styles.error}>{fieldError?.message}</p>}
      </div>
    </section>
  );
};

const NotesProperty = () => {
  return (
    <section className={`${styles.property} ${styles.notes}`}>
      <div className={classnames(styles.container, styles.main)}>
        <label>Notes</label>
        <div className={styles.toolbar}>
          <div className={styles.count}>
            <div className={styles.background} />
            <div className={styles.text}>+99</div>
          </div>
          <div className={`${styles.container} ${styles.buttons}`}>
            <button type="button">
              <EyeIcon fill={styles.toolbarButtonColor} />
            </button>
            <button type="button">
              <EditIcon fill={styles.toolbarButtonColor} />
            </button>
            <button type="button">
              <AddIcon fill={styles.toolbarButtonColor} />
            </button>
          </div>
        </div>
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        <p>Additional Message</p>
      </div>
    </section>
  );
};

const TagsProperty = () => {
  const { control } = useFormContext<NewAccountFormValues>();
  const { fields, append, remove } = useFieldArray({
    control: control,
    name: 'tags',
  });

  const {
    register,
    handleSubmit,
    reset,
    trigger,
    watch,
    formState: { errors },
  } = useForm<{ name: string }>();

  const newTagNameField = register('name', {
    required: 'Tags should contain some text',
    pattern: {
      value: /^[A-Za-z0-9-_ ]*$/,
      message:
        'Tags should only contain letters, numbers, whitespaces, dashes and underscores.',
    },
    validate: {
      unique: (newName) =>
        !fields.some((existing) => existing.name == newName) ||
        'The tag is being used already',
      firstLetter: (newName) =>
        /^[A-Za-z'-9-_]*$/.test(newName[0]) ||
        'Tags should not start with a whitespace',
    },
  });
  const newTagName = watch('name');

  const onNewTagSubmit: SubmitHandler<{ name: string }> = (data) => {
    append({ name: data.name });
    reset({ name: '' });
  };

  const [selection, setSelection] = useState<number[]>([]);
  const handleTagSelection = (index: number) => () =>
    setSelection((data) => [...data, index]);
  const handleTagDeselection = (index: number) => () =>
    setSelection((data) => data.filter((x) => x != index));

  const deleteSelectedTags = () => {
    console.log(selection);
    remove(selection);
    setSelection([]);
  };

  const onNewTagFormFocus = () => newTagName && trigger();
  const onNewTagFormBlur = () => reset({}, { keepValues: true });

  useEffect(() => {
    if (newTagName) {
      trigger('name');
    } else {
      reset({}, { keepValues: true });
    }
  }, [newTagName, trigger, reset]);

  return (
    <section className={classnames(styles.property, styles.tags)}>
      <div
        className={classnames(styles.container, styles.main)}
        onFocus={onNewTagFormFocus}
        onBlur={onNewTagFormBlur}
      >
        <form id="new-tag" hidden onSubmit={handleSubmit(onNewTagSubmit)} />
        <label htmlFor={newTagNameField.name}>Tags</label>
        <input
          {...newTagNameField}
          form="new-tag"
          placeholder="Add a tag..."
          autoComplete="off"
          className={`${styles.input} ${styles.simple}`}
          aria-invalid={errors.name ? true : false}
        />
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        {errors.name && <p className={styles.error}>{errors.name?.message}</p>}
      </div>
      <div className={styles.editor}>
        {fields.length > 0 && (
          <div className={styles.toolbar}>
            <p>Select tags to edit.</p>
            <div>
              <button type="button" onClick={deleteSelectedTags}>
                <DeleteBinIcon fill={styles.toolbarButtonColor} />
              </button>
            </div>
          </div>
        )}
        <div className={`${styles.container} ${styles.values}`}>
          {fields.map((field, index) => (
            <TagValue
              key={field.id}
              onTagSelection={handleTagSelection(index)}
              onTagDeselection={handleTagDeselection(index)}
            >
              {field.name}
            </TagValue>
          ))}
        </div>
      </div>
    </section>
  );
};

const TagValue = ({
  children,
  onTagSelection,
  onTagDeselection,
}: TagValueProps) => {
  const textRef = createRef<HTMLParagraphElement>();
  const [clipped, setClipped] = useState<boolean>(false);
  const [selected, setSelected] = useState<boolean>(false);

  const onClickToToggleSelect = () => {
    if (!selected && onTagSelection) {
      onTagSelection();
    }
    if (selected && onTagDeselection) {
      onTagDeselection();
    }
    setSelected(!selected);
  };

  useEffect(() => {
    const { scrollWidth, scrollHeight, clientWidth, clientHeight } =
      textRef.current ?? HTMLParagraphElement.prototype;

    const isOverflowing =
      scrollHeight > clientHeight || scrollWidth > clientWidth;

    setClipped(isOverflowing);
  }, [clipped, textRef]);

  return (
    <div
      className={styles.value}
      onClick={onClickToToggleSelect}
      data-tag-selected={selected}
      data-tag-clipped={clipped}
    >
      <div className={styles.hole} />
      <p ref={textRef}>{children}</p>
      <div className={styles.rightClipGradient} />
    </div>
  );
};

interface TagValueProps {
  children: ReactNode | ReactNode[] | undefined;
  onTagSelection?: () => void;
  onTagDeselection?: () => void;
}

export default NewAccountForm;
