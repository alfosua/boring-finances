import Image from 'next/image';
import { createRef, useEffect, useState } from 'react';
import { AddIcon, EyeIcon, EditIcon, DeleteBinIcon } from '../icons';
import styles from '../../styles/Form.module.scss';

const NewAccountForm = ({ className }: any) => {
  const finalClassName = className
    ? `${styles.form} ${className}`
    : styles.form;

  const submitAccount = (event: any) => {
    event.preventDefault();

    const formData = {
      title: event.target.title.value,
      code: event.target.code.value,
      type: event.target.type.value,
    };

    console.log('form data for creation:', JSON.stringify(formData, null, 4));
  };

  return (
    <form onSubmit={submitAccount} className={finalClassName}>
      <header>
        <h2>New Account</h2>
        <hr />
      </header>
      <section className={styles.container}>
        <GeneralSection />
        <AnnotationsSection />
      </section>
      <footer>
        <button type="submit" className={`${styles.button} ${styles.submit}`}>
          Create
        </button>
      </footer>
    </form>
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
      <div className={`${styles.container} ${styles.main}`}>
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
  return (
    <section className={styles.property}>
      <div className={`${styles.container} ${styles.main}`}>
        <label htmlFor="title">Title</label>
        <input
          required
          id="title"
          type="text"
          placeholder="Untitled"
          className={`${styles.input} ${styles.simple}`}
        />
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        <p>Additional Message</p>
      </div>
    </section>
  );
};

const CodeProperty = () => {
  return (
    <section className={styles.property}>
      <div className={`${styles.container} ${styles.main}`}>
        <label htmlFor="code">Code</label>
        <input
          id="code"
          type="text"
          placeholder="Optional..."
          className={`${styles.input} ${styles.simple}`}
        />
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        <p>Additional Message</p>
      </div>
    </section>
  );
};

const TypeProperty = () => {
  return (
    <section className={styles.property}>
      <div className={`${styles.container} ${styles.main}`}>
        <label htmlFor="type">Type</label>
        <select
          required
          id="type"
          placeholder="Select..."
          className={`${styles.input} ${styles.simple}`}
        >
          <option value={1}>Equity</option>
          <option value={2}>Asset</option>
          <option value={3}>Liability</option>
          <option value={4}>Income</option>
          <option value={5}>Expense</option>
          <option value={6}>Exchange</option>
        </select>
      </div>
      <div className={`${styles.container} ${styles.additional}`}>
        <p>Additional Message</p>
      </div>
    </section>
  );
};

const NotesProperty = () => {
  return (
    <section className={`${styles.property} ${styles.notes}`}>
      <div className={`${styles.container} ${styles.main}`}>
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
  return (
    <section className={`${styles.property} ${styles.tags}`}>
      <div className={`${styles.container} ${styles.main}`}>
        <label htmlFor="tag">Tags</label>
        <input
          id="tag"
          type="text"
          placeholder="Add a tag..."
          className={`${styles.input} ${styles.simple}`}
        />
      </div>
      <div className={styles.editor}>
        <div className={styles.toolbar}>
          <p>Select tags to edit.</p>
          <div>
            <button type="button">
              <DeleteBinIcon fill={styles.toolbarButtonColor} />
            </button>
          </div>
        </div>
        <div className={`${styles.container} ${styles.values}`}>
          <TagValue>tag 1</TagValue>
          <TagValue>something</TagValue>
          <TagValue>
            long tags will be clipped to avoid strange overlapping
          </TagValue>
          <TagValue>selected</TagValue>
          <TagValue>lol</TagValue>
          <TagValue>x</TagValue>
        </div>
      </div>
    </section>
  );
};

const TagValue = ({ children }: TagValueProps) => {
  const textRef = createRef<HTMLParagraphElement>();
  const [clipped, setClipped] = useState<boolean>(false);
  const [selected, setSelected] = useState<boolean>(false);

  const onClickToToggleSelect = () => {
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
  children: any;
}

export default NewAccountForm;
